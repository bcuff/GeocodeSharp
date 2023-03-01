using GeocodeSharp.Google;
using System.Linq;
using System.Reflection;
using Xunit;

namespace GeocodeSharp.Tests.Serialization {
  public class SerializationTest {
    private static readonly GeocodeResponse ResponseMock = new GeocodeResponse {
      Results = new[] {
        new GeocodeResult {
          AddressComponents =  new []{
            new AddressComponent {
              LongName = "My Long Name",
              ShortName = "My Short Name",
              Types = new [] {"type1", "type2"}
            }
          },
          FormattedAddress = "My Formatted Address",
          Geometry = new Geometry {
            Bounds = new GeoViewport {
              Northeast = new GeoCoordinate {Latitude = 0, Longitude=1},
              Southwest = new GeoCoordinate { Latitude = 1, Longitude=2}
            },
            Location = new GeoCoordinate {Latitude = 51, Longitude=-1},
            LocationTypeText = "My Location Type",
            Viewport  = new GeoViewport {
              Northeast = new GeoCoordinate {Latitude = 10, Longitude=11},
              Southwest = new GeoCoordinate { Latitude = 11, Longitude=12}
            }
          },
          PartialMatch = true,
          PlaceId = "My Place ID",
          Types = new[] {"type1", "type2", "type3"},
        }
      },
      StatusText = "OK",
    };

    private T SerializeAndDeserialize<T>(T value) {
      var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);

      return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    [Fact]
    public void TestAddressComponentSerialization() {
      var expected = ResponseMock.Results.First().AddressComponents.First();
      var actual = SerializeAndDeserialize(expected);

      Assert.Equal(expected.LongName, actual.LongName);
      Assert.Equal(expected.ShortName, actual.ShortName);
      Assert.Equal(expected.Types, actual.Types);
    }

    [Fact]
    public void TestGeocodeResponseSerialization() {
      var expected = ResponseMock;
      var actual = SerializeAndDeserialize(expected);

      Assert.Equal(expected.StatusText, actual.StatusText);
      Assert.NotEmpty(actual.Results); // Tested by TestGeocodeResultSerialization
    }

    [Fact]
    public void TestGeocodeResultSerialization() {
      var expected = ResponseMock.Results.First();
      var actual = SerializeAndDeserialize(expected);

      Assert.NotEmpty(actual.AddressComponents); // Tested by TestAddressComponentSerialization
      Assert.Equal(expected.FormattedAddress, actual.FormattedAddress);
      Assert.NotNull(actual.Geometry); // Tested by TestGeometrySerialization
      Assert.Equal(expected.PartialMatch, actual.PartialMatch);
      Assert.Equal(expected.PlaceId, actual.PlaceId);
      Assert.Equal(expected.Types, actual.Types);
    }

    [Fact]
    public void TestGeoCoordinateSerialization() {
      var expected = ResponseMock.Results.First().Geometry.Location;
      var actual = SerializeAndDeserialize(expected);

      Assert.Equal(expected.Latitude, actual.Latitude);
      Assert.Equal(expected.Longitude, actual.Longitude);
    }

    [Fact]
    public void TestGeometrySerialization() {
      var expected = ResponseMock.Results.First().Geometry;
      var actual = SerializeAndDeserialize(expected);

      Assert.NotNull(actual.Bounds); // Tested by TestGeoViewportSerialization
      Assert.NotNull(actual.Location); // Tested by TestGeoCoordinateSerialization
      Assert.Equal(expected.LocationTypeText, actual.LocationTypeText);
      Assert.Equal(expected.LocationType, actual.LocationType);
      Assert.NotNull(actual.Viewport); // Tested by TestGeoViewportSerialization
    }

    [Fact]
    public void TestGeoViewportSerialization() {
      var expected = ResponseMock.Results.First().Geometry.Viewport;
      var actual = SerializeAndDeserialize(expected);

      Assert.Equal(expected.Northeast.Latitude, actual.Northeast.Latitude);
      Assert.Equal(expected.Northeast.Longitude, actual.Northeast.Longitude);
      Assert.Equal(expected.Southwest.Latitude, actual.Southwest.Latitude);
      Assert.Equal(expected.Southwest.Longitude, actual.Southwest.Longitude);
    }

    [Fact]
    public void TestJsonPropertyNames() {
      var allTypes = typeof(AddressComponent).Assembly.GetTypes();

      foreach (var t in allTypes) {
        foreach (var prop in t.GetProperties()) {
          var newtonName = prop.GetCustomAttribute<Newtonsoft.Json.JsonPropertyAttribute>();
          var txtName = prop.GetCustomAttribute<System.Text.Json.Serialization.JsonPropertyNameAttribute>();

          // These tests are pretty ugly, but looping through all the properties of the models ensure that we
          // have matching Newtonsoft and System.Text.Json attributes for every proeprty
          Assert.False(newtonName != null && txtName == null, $"{t.FullName}.{prop.Name} only has a Newtonsoft.Json.JsonPropertyAttribute");
          Assert.False(txtName != null && newtonName == null, $"{t.FullName}.{prop.Name} only has a System.Text.Json.Serialization.JsonPropertyNameAttribute");
          Assert.True(newtonName?.PropertyName == txtName?.Name, $"{t.FullName}.{prop.Name} expected '{newtonName?.PropertyName}', but got '{txtName?.Name}'");
          var newtonIgnore = prop.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>();
          var txtIgnore = prop.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>();

          Assert.False(newtonIgnore != null && txtIgnore == null, $"{t.FullName}.{prop.Name} only has a Newtonsoft.Json.JsonIgnoreAttribute");
          Assert.False(txtIgnore != null && newtonIgnore == null, $"{t.FullName}.{prop.Name} only has a System.Text.Json.Serialization.JsonIgnoreAttribute");
        }
      }

    }

  }
}
