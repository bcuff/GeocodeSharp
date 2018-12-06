using System.Threading.Tasks;

namespace GeocodeSharp.Google
{
    public interface IGeocodeClient
    {

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="address">The street address that you want to geocode, in the format used by the national postal service of the country concerned. Additional address elements such as business names and unit, suite or floor numbers should be avoided.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// /// <param name="language"> The language in which to return results. Address components will all be returned in the same language, which is chosen from the first component. Should names not be available in the preferred language, the closest match will be used.</param>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <returns>The geocode response.</returns>
        Task<GeocodeResponse> GeocodeAddress(string address, string region = null, ComponentFilter filter = null, string language = null);

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="latitude">The latitude value specifying the location for which you wish to obtain the closest, human-readable address.</param>
        /// <param name="longtitude">The longtitude value specifying the location for which you wish to obtain the closest, human-readable address.</param>
        /// <param name="language"> The language in which to return results. Address components will all be returned in the same language, which is chosen from the first component. Should names not be available in the preferred language, the closest match will be used.</param>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <returns>The geocode response.</returns>
        Task<GeocodeResponse> GeocodeAddress(double latitude, double longtitude, string language = null);

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="address">The street address that you want to geocode, in the format used by the national postal service of the country concerned. Additional address elements such as business names and unit, suite or floor numbers should be avoided.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// /// <param name="language"> The language in which to return results. Address components will all be returned in the same language, which is chosen from the first component. Should names not be available in the preferred language, the closest match will be used.</param>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <returns>The geocode response as JSON.</returns>
        Task<string> GeocodeAddressJson(string address, string region = null, ComponentFilter filter = null, string language = null);

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// <returns>The geocode response.</returns>
        Task<GeocodeResponse> GeocodeComponentFilter(ComponentFilter filter, string region = null);

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// <returns>The geocode response as JSON.</returns>
        Task<string> GeocodeComponentFilterJson(ComponentFilter filter, string region = null);

    }
}
