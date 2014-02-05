using System;
using System.Linq;
using Newtonsoft.Json;

namespace GeocodeSharp.Google
{
    public class GeocodeResult
    {
        [JsonProperty("address_components")]
        public AddressComponent[] AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("partial_match")]
        public bool PartialMatch { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }
}
