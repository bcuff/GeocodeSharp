using System;
using System.Linq;
using System.Threading.Tasks;
using GeocodeSharp.Google;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeocodeSharp.Tests.Google
{
    [TestClass]
    public class GeocodeClientTest
    {
        [TestMethod]
        public async Task TestComponentFilter()
        {
            var client = new GeocodeClient();
            var result = await client.GeocodeAddress("santa cruz");
            Assert.IsTrue(result.Results.Count() > 1, "'santa cruz' should return multiple results when used without component filter.");
            result = await client.GeocodeAddress("santa cruz", filter: new ComponentFilter { Country = "es" });
            Assert.IsTrue(result.Results.Count() == 1, "'santa cruz' should return singler result when used with Country=es filter");
        }

        [TestMethod]
        public async Task TestGeocodeAddressZeroResults()
        {
            var client = new GeocodeClient();
            var result = await client.GeocodeAddress(Guid.NewGuid().ToString("N"));
            Assert.AreEqual(GeocodeStatus.ZeroResults, result.Status);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public async Task TestGeocodeAddressWithNullAddress()
        {
            var client = new GeocodeClient();
            await client.GeocodeAddress(null);
            Assert.Fail();
        }

        [TestMethod]
        public async Task TestGeocodeAddressWithPartialMatch()
        {
            const string address = "21 Henr St, Bristol, UK";
            var client = new GeocodeClient();
            var result = await client.GeocodeAddress(address);
            Assert.AreEqual(GeocodeStatus.Ok, result.Status);
            Assert.AreEqual(true, result.Results.All(r => r.PartialMatch));
            Assert.AreEqual(true, result.Results.Length > 0);
        }

        [TestMethod]
        [Ignore]  // Unignore after adding client credentials.
        public async Task TestGeocodeAddressWithPartialMatchWithApiForWork()
        {
            var clientId = "[ADD-CLIENT-ID-HERE]";
            var cryptoKey = "[ADD-CRYPTO_KEY_HERE]";;
            const string address = "21 Henr St, Bristol, UK";
            var client = new GeocodeClient(clientId, cryptoKey);
            var result = await client.GeocodeAddress(address);
            Assert.AreEqual(GeocodeStatus.Ok, result.Status);
            Assert.AreEqual(true, result.Results.All(r => r.PartialMatch));
            Assert.AreEqual(true, result.Results.Length > 0);
        }
        
        [TestMethod]
        public async Task TestTestGeocodeAddressWithExactMatch()
        {
            const string address = "21 Henrietta St, Bristol, UK";
            var client = new GeocodeClient();
            var response = await client.GeocodeAddress(address);
            Assert.AreEqual(GeocodeStatus.Ok, response.Status);
            Assert.AreEqual(false, response.Results.All(r => r.PartialMatch));
            Assert.AreEqual(true, response.Results.Length == 1);

            var result = response.Results[0];
            Assert.AreEqual("21 Henrietta St, Bristol, City of Bristol BS5 6HU, UK", result.FormattedAddress);
            Assert.AreEqual(51, (int)result.Geometry.Location.Latitude);
            Assert.AreEqual(-2, (int)result.Geometry.Location.Longitude);
            Assert.AreEqual("ChIJS_spyTiOcUgRfgVi31-TvpY", result.PlaceId);
            Assert.IsTrue(result.Types.Contains("street_address"));
        }

        [TestMethod]
        public async Task TestGeocodeAddressWithRegion()
        {
            var client = new GeocodeClient();
            var result = await client.GeocodeAddress("London", region: "ca");
            Assert.AreEqual(GeocodeStatus.Ok, result.Status);
            Assert.AreEqual("London, ON, Canada", result.Results.First().FormattedAddress);

            result = await client.GeocodeAddress("London", region: "uk");
            Assert.AreEqual(GeocodeStatus.Ok, result.Status);
            Assert.AreEqual("London, UK", result.Results.First().FormattedAddress);
        }

        [TestMethod]
        public async Task TestGeocodeAddressWithLanguage()
        {
            var client = new GeocodeClient();
            var spanishResponse = await client.GeocodeAddress("Madrid, Caja Mágica", language: "es");

            var result = spanishResponse.Results.First();
            Assert.AreEqual("Área Metropolitalitana y Corredor del Henares", result.AddressComponents[2].LongName);
            Assert.AreEqual("España", result.AddressComponents[5].LongName);
        }
    }
}
