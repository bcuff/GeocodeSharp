using System;
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

        /// <summary>
        /// Initialize GeocodeClient without a Google API key and use default annonymouse access.
        /// NOTE: Throttling may apply.
        /// </summary>
        public GeocodeClient()
        {
            _mode = UsageMode.Free;
        }

        /// <summary>
        /// Initialize GeocodeClient with your Google API key to utilize it in the requests to Google and bypass the default annonymous throttling.
        /// </summary>
        /// <param name="apiKey">Google Maps API Key</param>
        public GeocodeClient(string apiKey)
        {
            _clientKey = apiKey;
            _mode = UsageMode.ClientKey;
        }

        public GeocodeClient(string clientId, string cryptoKey)
        {
            _clientId = clientId;
            _cryptoKey = cryptoKey;
            _mode = UsageMode.ApiForWork;
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
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException("address");
            
            switch (_mode)
            {
                case UsageMode.Free:
                    return BuildFreeUrl(address, region);
                case UsageMode.ClientKey:
                    return BuildClientKeyUrl(address, region);
                case UsageMode.ApiForWork:
                    return BuildApiForWorkUrl(address, region);
                default:
                    return BuildFreeUrl(address, region);
            }
        }

        private string GetAddressPortion(string address, string region)
        {
            if (string.IsNullOrWhiteSpace(region))
            {
                return string.Format("address={0}", Uri.EscapeDataString(address));
            }
            return string.Format("address={0}&region={1}", Uri.EscapeDataString(address), Uri.EscapeDataString(region));
        }

        private string BuildFreeUrl(string address, string region)
        {
            var addressPortion = GetAddressPortion(address, region);
            return string.Format("{0}{1}{2}", _domain, _apiPath, addressPortion);
        }

        private string BuildClientKeyUrl(string address, string region)
        {
            var addressPortion = GetAddressPortion(address, region);
            return string.Format("{0}{1}{2}&key={3}", _domain, _apiPath, addressPortion, _clientKey);
        }

        private string BuildApiForWorkUrl(string address, string region)
        {
            var addressPortion = GetAddressPortion(address, region);
            var cryptoBytes = Convert.FromBase64String(_cryptoKey.Replace("-", "+").Replace("_", "/"));
            var hashThis = string.Format("{0}{1}&client={2}", _apiPath, addressPortion, _clientId);
            var hashThisBytes = Encoding.ASCII.GetBytes(hashThis);
            using (var sha1 = new HMACSHA1(cryptoBytes))
            {
                var hash = sha1.ComputeHash(hashThisBytes);
                var signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
                return string.Format("{0}{1}&signature={2}", _domain, hashThis, signature);
            }
        }
    }
}
