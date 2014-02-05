using System;
using System.Linq;
using Newtonsoft.Json;

namespace GeocodeSharp.Google
{
    public class GeoCoordinate
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}
