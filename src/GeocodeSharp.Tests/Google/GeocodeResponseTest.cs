using System;
using System.Linq;
using GeocodeSharp.Google;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeocodeSharp.Tests.Google
{
    [TestClass]
    public class GeocodeResponseTest
    {
        [TestMethod]
        public void TestStatusSerialization()
        {
            var before = new GeocodeResponse
            {
                StatusText = "OK",
            };
            var json = JsonConvert.SerializeObject(before);
            dynamic after = JObject.Parse(json);
            Assert.AreEqual("OK", (string)after.status);
            Assert.AreEqual(null, after.Status);
        }
    }
}
