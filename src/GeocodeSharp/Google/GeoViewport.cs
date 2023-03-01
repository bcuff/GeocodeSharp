using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GeocodeSharp.Google
{
    public class GeoViewport
    {
        [JsonProperty("northeast"), JsonPropertyName("northeast")]
        public GeoCoordinate Northeast { get; set; }

        [JsonProperty("southwest"), JsonPropertyName("southwest")]
        public GeoCoordinate Southwest { get; set; }
    }
}
