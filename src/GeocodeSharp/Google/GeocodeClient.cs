﻿using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeocodeSharp.Google
{
    /// <summary>
    /// Encapsulates methods for executing geocode requests.
    /// </summary>
    public class GeocodeClient : IGeocodeClient
    {
        private readonly UsageMode _mode;
        private const string _domain = "https://maps.googleapis.com";
        private const string _apiPath = "/maps/api/geocode/json?";
        private readonly string _clientId;
        private readonly string _cryptoKey;
        private readonly string _clientKey;

        private IGeocodeProxyProvider _proxyProvider;

        /// <summary>
        /// Initialize GeocodeClient without a Google API key and use default anonymous access.
        /// NOTE: Throttling may apply.
        /// </summary>
        public GeocodeClient()
        {
            _mode = UsageMode.Free;
            _proxyProvider = new DefaultGeocodeProxyProvider();
        }

        public GeocodeClient(IGeocodeProxyProvider proxyProvider): this()
        {
            _proxyProvider = proxyProvider;
        }

        /// <summary>
        /// Initialize GeocodeClient with your Google API key to utilize it in the requests to Google and bypass the default anonymous throttling.
        /// </summary>
        /// <param name="apiKey">Google Maps API Key</param>
        public GeocodeClient(string apiKey): this()
        {
            _clientKey = apiKey;
            _mode = UsageMode.ClientKey;
        }

        public GeocodeClient(IGeocodeProxyProvider proxyProvider, string apiKey): this(apiKey)
        {
            _proxyProvider = proxyProvider;
        }

        /// <summary>
        /// Initialize GeocodeClient with your Google API key to utilize it in the requests to Google and bypass the default anonymous throttling.
        /// </summary>
        /// <param name="clientId">The client ID. Applicable when using Maps API for Work.</param>
        /// <param name="cryptoKey">The base64 encoded crypto key. Applicable when using Maps API for Work.</param>
        /// <remarks>
        /// See - https://developers.google.com/maps/documentation/business/webservices/#client_id
        /// </remarks>
        public GeocodeClient(string clientId, string cryptoKey): this()
        {
            _clientId = clientId;
            _cryptoKey = cryptoKey;
            _mode = UsageMode.ApiForWork;
        }

        public GeocodeClient(IGeocodeProxyProvider proxyProvider, string clientId, string cryptoKey): this(clientId, cryptoKey)
        {
            _proxyProvider = proxyProvider;
        }

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="address">The street address that you want to geocode, in the format used by the national postal service of the country concerned. Additional address elements such as business names and unit, suite or floor numbers should be avoided.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// <param name="language"> The language in which to return results. Address components will all be returned in the same language, which is chosen from the first component. Should names not be available in the preferred language, the closest match will be used.</param>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <returns>The geocode response.</returns>
        public async Task<GeocodeResponse> GeocodeAddress(string address, string region = null, ComponentFilter filter = null, string language = null)
        {
            var request = BuildRequest(address, region, language, filter);
            var response = await DoRequestAsync(request);
            return JsonConvert.DeserializeObject<GeocodeResponse>(response);
        }

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="latitude">The latitude value specifying the location for which you wish to obtain the closest, human-readable address.</param>
        /// <param name="longitude">The longitude value specifying the location for which you wish to obtain the closest, human-readable address.</param>
        /// <param name="language"> The language in which to return results. Address components will all be returned in the same language, which is chosen from the first component. Should names not be available in the preferred language, the closest match will be used.</param>
        /// <param name="locationTypeFilter">The location type filter parameter does not restrict the search to the specified location type(s). Rather, the location type filter acts as a post-search filter: the API fetches all results for the specified latitude and longitude, then discards those results that do not match the specified location type(s).</param>
        /// <param name="resultTypeFilter">The result type filter parameter does not restrict the search to the specified address type(s). Rather, the result type filter acts as a post-search filter: the API fetches all results for the specified latitude and longitude, then discards those results that do not match the specified address type(s).</param>
        /// <returns>The geocode response.</returns>
        public async Task<GeocodeResponse> GeocodeAddress(double latitude, double longitude, string language = null, ResultTypeFilter resultTypeFilter = null, LocationTypeFilter locationTypeFilter = null)
        {
            if (_mode == UsageMode.Free && resultTypeFilter != null)
            {
                throw new ArgumentException("You can't use result type filter in free mode, consider running your Geocoding client with API key or client key.");
            }

            if (_mode == UsageMode.Free && locationTypeFilter != null)
            {
                throw new ArgumentException("You can't use location type filter in free mode, consider running your Geocoding client with API key or client key.");
            }

            var request = BuildRequest(latitude, longitude, language, resultTypeFilter, locationTypeFilter);
            var response = await DoRequestAsync(request);
            return JsonConvert.DeserializeObject<GeocodeResponse>(response);
        }

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="latitude">The latitude value specifying the location for which you wish to obtain the closest, human-readable address.</param>
        /// <param name="longitude">The longitude value specifying the location for which you wish to obtain the closest, human-readable address.</param>
        /// <param name="language"> The language in which to return results. Address components will all be returned in the same language, which is chosen from the first component. Should names not be available in the preferred language, the closest match will be used.</param>
        /// <param name="locationTypeFilter">The location type filter parameter does not restrict the search to the specified location type(s). Rather, the location type filter acts as a post-search filter: the API fetches all results for the specified latitude and longitude, then discards those results that do not match the specified location type(s).</param>
        /// <param name="resultTypeFilter">The result type filter parameter does not restrict the search to the specified address type(s). Rather, the result type filter acts as a post-search filter: the API fetches all results for the specified latitude and longitude, then discards those results that do not match the specified address type(s).</param>
        /// <returns>The geocode response as JSON.</returns>
        public async Task<string> GeocodeAddressJson(double latitude, double longitude, string language = null, ResultTypeFilter resultTypeFilter = null, LocationTypeFilter locationTypeFilter = null)
        {

            if (_mode == UsageMode.Free && resultTypeFilter != null)
            {
                throw new ArgumentException("You can't use result type filter in free mode, consider running your Geocoding client with API key or client key.");
            }

            if (_mode == UsageMode.Free && locationTypeFilter != null)
            {
                throw new ArgumentException("You can't use location type filter in free mode, consider running your Geocoding client with API key or client key.");
            }

            var request = BuildRequest(latitude, longitude, language, resultTypeFilter, locationTypeFilter);
            return await DoRequestAsync(request);
        }

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="address">The street address that you want to geocode, in the format used by the national postal service of the country concerned. Additional address elements such as business names and unit, suite or floor numbers should be avoided.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// <param name="language"> The language in which to return results. Address components will all be returned in the same language, which is chosen from the first component. Should names not be available in the preferred language, the closest match will be used.</param>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <returns>The geocode response as JSON.</returns>
        public async Task<string> GeocodeAddressJson(string address, string region = null, ComponentFilter filter = null, string language = null)
        {
            var request = BuildRequest(address, region, language, filter);
            return await DoRequestAsync(request);
        }


        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// <returns>The geocode response.</returns>
        public async Task<GeocodeResponse> GeocodeComponentFilter(ComponentFilter filter, string region = null)
        {
            var request = BuildRequest(filter, region);
            var response = await DoRequestAsync(request);
            return JsonConvert.DeserializeObject<GeocodeResponse>(response);
        }

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="filter">A component filter for which you wish to obtain a geocode. The component filter swill fully restrict the results from the geocoder. Only the results that match all the filters will be returned. Each address component can only be specified either in the address parameter or as a component filter, but not both. Doing so may result in ZERO_RESULTS.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// <returns>The geocode response.</returns>
        public async Task<string> GeocodeComponentFilterJson(ComponentFilter filter, string region = null)
        {
            var request = BuildRequest(filter, region);
            return await DoRequestAsync(request);            
        }

        private async Task<string> DoRequestAsync(HttpWebRequest request)
        {
            string json;
            using (var ms = new MemoryStream())
            {
                using (var response = await request.GetResponseAsync())
                using (var body = response.GetResponseStream())
                {
                    if (body != null)
                        await body.CopyToAsync(ms);
                }

                json = Encoding.UTF8.GetString(ms.ToArray());
            }
            return json;
        }

        
        private HttpWebRequest BuildRequest(ComponentFilter filter, string region)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            var addressPortion = BuildAddressPortion(filter, region);
            var authPortion = BuildAuthPortion(addressPortion);
            return _proxyProvider.CreateRequest(string.Format("{0}{1}{2}{3}", _domain, _apiPath, addressPortion, authPortion));
        }

        private HttpWebRequest BuildRequest(string address, string region, string language, ComponentFilter filter)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException("address");
            var addressPortion = BuildAddressPortion(address, region, language, filter);
            var authPortion = BuildAuthPortion(addressPortion);
            return _proxyProvider.CreateRequest(string.Format("{0}{1}{2}{3}", _domain, _apiPath, addressPortion, authPortion));
        }

        private HttpWebRequest BuildRequest(double latitude, double longitude, string language = null, ResultTypeFilter resultTypeFilter = null, LocationTypeFilter locationTypeFilter = null)
        {
            var latLngPortion = BuilLatLngPortion(latitude, longitude, language, resultTypeFilter, locationTypeFilter);
            var authPortion = BuildAuthPortion(latLngPortion);
            return _proxyProvider.CreateRequest(string.Format("{0}{1}{2}{3}", _domain, _apiPath, latLngPortion, authPortion));
        }

        private string BuildAuthPortion(string addressPortion)
        {
            switch (_mode)
            {
                case UsageMode.Free:
                    return string.Empty;
                case UsageMode.ClientKey:
                    return string.Format("&key={0}", _clientKey);
                case UsageMode.ApiForWork:
                    return BuildApiForWorkUrl(addressPortion);
                default:
                    return string.Empty;
            }
        }

        private string BuildAddressPortion(ComponentFilter filter, string region)
        {
            var filterString = filter.ToUrlParameters();
            if(string.IsNullOrWhiteSpace(filterString))
                throw new ArgumentException("Component filter doesn't contain any component", "filter");
            var addressPortion = string.Format("components={0}", filterString);
            if (!string.IsNullOrWhiteSpace(region))
            {
                addressPortion += string.Format("&region={0}", Uri.EscapeDataString(region));
            }

            return addressPortion;
        }

        private string BuilLatLngPortion(double latitude, double longitude, string language, ResultTypeFilter resultTypeFilter = null, LocationTypeFilter locationTypeFilter = null)
        {
            var latLngPortion = string.Format("latlng={0},{1}", 
                Uri.EscapeDataString(latitude.ToString("0.######", CultureInfo.InvariantCulture)),
                Uri.EscapeDataString(longitude.ToString("0.######", CultureInfo.InvariantCulture)));

            if (!string.IsNullOrWhiteSpace(language))
            {
                latLngPortion += string.Format("&language={0}", Uri.EscapeDataString(language));
            }

            if (resultTypeFilter != null)
            {
                latLngPortion += string.Format("&result_type={0}", Uri.EscapeDataString(resultTypeFilter.ToUrlParameters()));
            }

            if (locationTypeFilter != null)
            {
                latLngPortion += string.Format("&location_type={0}", Uri.EscapeDataString(locationTypeFilter.ToUrlParameters()));
            }

            return latLngPortion;
        }

        private string BuildAddressPortion(string address, string region, string language, ComponentFilter filter)
        {
            var addressPortion = string.Format("address={0}", Uri.EscapeDataString(address));
            if (!string.IsNullOrWhiteSpace(region))
            {
                addressPortion += string.Format("&region={0}", Uri.EscapeDataString(region));
            }

            if (!string.IsNullOrWhiteSpace(language))
            {
                addressPortion += string.Format("&language={0}", Uri.EscapeDataString(language));
            }

            if (filter != null)
            {
                var filterString = filter.ToUrlParameters();
                if (!string.IsNullOrWhiteSpace(filterString))
                {
                    addressPortion += string.Format("&components={0}", filterString);
                }
            }

            return addressPortion;
        }

        private string BuildApiForWorkUrl(string addressPortion)
        {
            var cryptoBytes = Convert.FromBase64String(_cryptoKey.Replace("-", "+").Replace("_", "/"));
            var hashThis = string.Format("{0}{1}&client={2}", _apiPath, addressPortion, _clientId);
            var hashThisBytes = Encoding.ASCII.GetBytes(hashThis);
            using (var sha1 = new HMACSHA1(cryptoBytes))
            {
                var hash = sha1.ComputeHash(hashThisBytes);
                var signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
                return string.Format("&client={0}&signature={1}", _clientId, signature);
            }
        }
    }
}
