using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GeocodeSharp.Google
{
    public class AddressComponent
    {
        [JsonProperty("long_name"), JsonPropertyName("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name"), JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types"), JsonPropertyName("types")]
        public string[] Types { get; set; }
    }
}
