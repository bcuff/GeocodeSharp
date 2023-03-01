using GeocodeSharp.Google;
using System.Reflection;
using Xunit;

namespace GeocodeSharp.Tests.Serialization {
  public class SerializationTest {
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
          var newtonIgnore = prop.GetCustomAttribute<Newtonsoft.Json.JsonIgnoreAttribute>() ;
          var txtIgnore = prop.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() ;

          Assert.False(newtonIgnore != null && txtIgnore == null, $"{t.FullName}.{prop.Name} only has a Newtonsoft.Json.JsonIgnoreAttribute");
          Assert.False(txtIgnore != null && newtonIgnore == null, $"{t.FullName}.{prop.Name} only has a System.Text.Json.Serialization.JsonIgnoreAttribute");
        }
      }

    }

  }
}
