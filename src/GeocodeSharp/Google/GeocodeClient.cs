using System;
using System.IO;
using System.Net;
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
        private readonly string _apiKey;
        private readonly string _baseUrl;

        /// <summary>
        /// Initialize GeocodeClient without a Google API key and use default annonymouse access.
        /// NOTE: Throttling may apply.
        /// </summary>
        public GeocodeClient()
        {
            _baseUrl = "http://maps.googleapis.com/maps/api/geocode/json?";
        }

        /// <summary>
        /// Initialize GeocodeClient with your Google API key to utilize it in the requests to Google and bypass the default annonymous throttling.
        /// </summary>
        /// <param name="apiKey">Google Maps API Key</param>
        public GeocodeClient(string apiKey)
        {
            _apiKey = apiKey;
            _baseUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?key={0}&", Uri.EscapeDataString(_apiKey));
        }

        /// <summary>
        /// Calls Google's geocode API with the specified address and optional region.
        /// https://developers.google.com/maps/documentation/geocoding/#GeocodingRequests
        /// </summary>
        /// <param name="address">The street address that you want to geocode, in the format used by the national postal service of the country concerned. Additional address elements such as business names and unit, suite or floor numbers should be avoided.</param>
        /// <param name="region">The region code, specified as a ccTLD ("top-level domain") two-character value. This parameter will only influence, not fully restrict, results from the geocoder.</param>
        /// <returns>The geocode response.</returns>
        public async Task<GeocodeResponse> GeocodeAddress(string address, string region = null)
        {
            var url = BuildUrl(address, region);

            string json;
            var request = WebRequest.CreateHttp(url);
            using (var ms = new MemoryStream())
            {
                using (var response = await request.GetResponseAsync())
                using (var body = response.GetResponseStream())
                {
                    if (body != null) await body.CopyToAsync(ms);
                }

                json = Encoding.UTF8.GetString(ms.ToArray());
            }
            return JsonConvert.DeserializeObject<GeocodeResponse>(json);
        }

        private string BuildUrl(string address, string region)
        {
            if (address == null) throw new ArgumentNullException("address");

            if (string.IsNullOrWhiteSpace(region))
            {
                return string.Concat(_baseUrl, string.Format("address={0}", Uri.EscapeDataString(address)));
            }

            return string.Concat(_baseUrl,
                string.Format("address={0}&region={1}", Uri.EscapeDataString(address), Uri.EscapeDataString(region)));
        }
    }
}
