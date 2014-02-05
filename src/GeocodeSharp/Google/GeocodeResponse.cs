using System;
using System.Linq;
using Newtonsoft.Json;

namespace GeocodeSharp.Google
{
    public class GeocodeResponse
    {
        [JsonProperty("results")]
        public GeocodeResult[] Results { get; set; }

        public GeocodeStatus Status
        {
            get
            {
                switch (StatusText)
                {
                    case "OK": return GeocodeStatus.Ok;
                    case "ZERO_RESULTS": return GeocodeStatus.ZeroResults;
                    case "OVER_QUERY_LIMIT": return GeocodeStatus.OverQueryLimit;
                    case "REQUEST_DENIED": return GeocodeStatus.RequestDenied;
                    case "INVALID_REQUEST": return GeocodeStatus.InvalidRequest;
                    case "UNKNOWN_ERROR": return GeocodeStatus.UnknownError;
                    default: return GeocodeStatus.Unexpected;
                }
            }
        }

        [JsonProperty("status")]
        public string StatusText { get; set; }
    }
}
