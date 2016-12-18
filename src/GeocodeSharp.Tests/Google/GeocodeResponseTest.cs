using System;
using System.Linq;
using GeocodeSharp.Google;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeocodeSharp.Tests.Google
{
    public class GeocodeResponseTest
    {
        [Fact]
        public void TestStatusSerialization()
        {
            var before = new GeocodeResponse
            {
                StatusText = "OK",
            };
            var json = JsonConvert.SerializeObject(before);
            dynamic after = JObject.Parse(json);
            Assert.Equal("OK", (string)after.status);
            Assert.Equal(null, after.Status);
        }
    }
}
