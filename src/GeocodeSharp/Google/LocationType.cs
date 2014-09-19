namespace GeocodeSharp.Google
{
    public enum LocationType
    {
        Rooftop,
        RangeInterpolated,
        GeometricCenter,
        Approximate,
        // in case the server returns back something unknown
        Unknown,
    }
}
