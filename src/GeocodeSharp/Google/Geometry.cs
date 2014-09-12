using Newtonsoft.Json;

namespace GeocodeSharp.Google
{
    public class Geometry
    {
        [JsonProperty("location")]
        public GeoCoordinate Location { get; set; }

        public LocationType LocationType
        {
            get
            {
                switch (LocationTypeText)
                {
                    case "ROOFTOP": return LocationType.Rooftop;
                    case "RANGE_INTERPOLATED": return LocationType.RangeInterpolated;
                    case "GEOMETRIC_CENTER": return LocationType.GeometricCenter;
                    case "APPROXIMATE": return LocationType.Approximate;
                    default: return LocationType.Unknown;
                }
            }
        }

        [JsonProperty("location_type")]
        public string LocationTypeText { get; set; }

        [JsonProperty("viewport")]
        public GeoViewport Viewport { get; set; }

        [JsonProperty("bounds")]
        public GeoViewport Bounds { get; set; }
    }
}
