using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeocodeSharp.Google;
using Xunit;

namespace GeocodeSharp.Tests.Google
{
    public class GeocodeClientTestFixture
    {
        public GeocodeClientTestFixture()
        {
            Client = new GeocodeClient();
        }
        public IGeocodeClient Client;
    }
    public class GeocodeClientTest : IClassFixture<GeocodeClientTestFixture>
    {
        readonly GeocodeClientTestFixture ClientFixture;
        
        public GeocodeClientTest(GeocodeClientTestFixture fixture)
        {
            //this is just to be sure we dont hit rate limit
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            ClientFixture = fixture;
        }

        [Fact]
        public async Task TestComponentFilter()
        {
            var result = await ClientFixture.Client.GeocodeAddress("santa cruz");
            Assert.True(result.Results.Count() > 1, "'santa cruz' should return multiple results when used without component filter.");
            result = await ClientFixture.Client.GeocodeAddress("santa cruz", filter: new ComponentFilter { Country = "es" });
            Assert.True(result.Results.Count() == 1, "'santa cruz' should return singler result when used with Country=es filter");
        }

        [Fact]
        public async Task TestGeocodeAddressZeroResults()
        {
            var result = await ClientFixture.Client.GeocodeAddress(Guid.NewGuid().ToString("N"));
            Assert.Equal(GeocodeStatus.ZeroResults, result.Status);
        }

        [Fact]
        public void TestGeocodeAddressWithNullAddress()
        {
            Assert.ThrowsAsync<ArgumentNullException>( () => ClientFixture.Client.GeocodeAddress(null));
        }

        [Fact]
        public async Task TestGeocodeAddressWithPartialMatch()
        {
            const string address = "21 Henr St, Bristol, UK";

            var result = await ClientFixture.Client.GeocodeAddress(address);
            Assert.Equal(GeocodeStatus.Ok, result.Status);
            Assert.Equal(true, result.Results.All(r => r.PartialMatch));
            Assert.Equal(true, result.Results.Length > 0);
        }

        //[Fact]
        public async Task TestGeocodeAddressWithPartialMatchWithApiForWork()
        {
            var clientId = "[ADD-CLIENT-ID-HERE]";
            var cryptoKey = "[ADD-CRYPTO_KEY_HERE]"; ;
            const string address = "21 Henr St, Bristol, UK";
            var client = new GeocodeClient(clientId, cryptoKey);
            var result = await client.GeocodeAddress(address);
            Assert.Equal(GeocodeStatus.Ok, result.Status);
            Assert.Equal(true, result.Results.All(r => r.PartialMatch));
            Assert.Equal(true, result.Results.Length > 0);
        }

        [Fact]
        public async Task TestTestGeocodeAddressWithExactMatch()
        {
            const string address = "21 Henrietta St, Bristol, UK";

            var response = await ClientFixture.Client.GeocodeAddress(address);
            Assert.Equal(GeocodeStatus.Ok, response.Status);
            Assert.Equal(false, response.Results.All(r => r.PartialMatch));
            Assert.Equal(true, response.Results.Length == 1);

            var result = response.Results[0];
            Assert.Equal("21 Henrietta St, Bristol BS5 6HU, UK", result.FormattedAddress);
            Assert.Equal(51, (int)result.Geometry.Location.Latitude);
            Assert.Equal(-2, (int)result.Geometry.Location.Longitude);
            Assert.Equal("ChIJS_spyTiOcUgRfgVi31-TvpY", result.PlaceId);
            Assert.True(result.Types.Contains("street_address"));
        }

        [Fact]
        public async Task TestGeocodeAddressWithRegion()
        {
            var result = await ClientFixture.Client.GeocodeAddress("London", region: "ca");
            Assert.Equal(GeocodeStatus.Ok, result.Status);
            Assert.Equal("London, ON, Canada", result.Results.First().FormattedAddress);

            result = await ClientFixture.Client.GeocodeAddress("London", region: "uk");
            Assert.Equal(GeocodeStatus.Ok, result.Status);
            Assert.Equal("London, UK", result.Results.First().FormattedAddress);
        }

        [Fact]
        public async Task TestGeocodeAddressWithLanguage()
        {
            var spanishResponse = await ClientFixture.Client.GeocodeAddress("Madrid, Caja Mágica", language: "es");

            var result = spanishResponse.Results.First();
            Assert.Equal("Área Metropolitalitana y Corredor del Henares", result.AddressComponents[2].LongName);
            Assert.Equal("España", result.AddressComponents[5].LongName);
        }

        [Fact]
        public async Task TestGeocodeReverseGeocoding()
        {
            var reverseGeocodingResponse = await ClientFixture.Client.GeocodeAddress(32.715736, -117.161087);

            var result = reverseGeocodingResponse.Results.First();
            Assert.Equal("402w Broadway, San Diego, CA 92101, USA", result.FormattedAddress);
        }

        [Fact]
        public async Task TestGeocodeReverseGeocodingWithLanguage()
        {
            var reverseGeocodingResponse = await ClientFixture.Client.GeocodeAddress(32.715736, -117.161087, language: "es");

            var result = reverseGeocodingResponse.Results.First();
            Assert.Equal("402w Broadway, San Diego, CA 92101, EE. UU.", result.FormattedAddress);
        }
    }
}