using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GeocodeSharp.Google
{
    public class GeocodeResult
    {
        [JsonProperty("address_components"), JsonPropertyName("address_components")]
        public AddressComponent[] AddressComponents { get; set; }

        [JsonProperty("formatted_address"), JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry"), JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("partial_match"), JsonPropertyName("partial_match")]
        public bool PartialMatch { get; set; }

        [JsonProperty("place_id"), JsonPropertyName("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("types"), JsonPropertyName("types")]
        public string[] Types { get; set; }
    }
}
