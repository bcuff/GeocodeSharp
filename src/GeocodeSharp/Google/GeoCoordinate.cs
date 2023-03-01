using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GeocodeSharp.Google
{
    public class GeoCoordinate
    {
        [JsonProperty("lat"), JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng"), JsonPropertyName("lng")]
        public double Longitude { get; set; }
    }
}
