using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeocodeSharp.Google
{
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
            _baseUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?key={0}", Uri.EscapeDataString(_apiKey));
            var builder = new StringBuilder();
        }

        public async Task<GeocodeResponse> GeocodeAddress(string address, bool sensor = false, string region = null)
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
                string.Format("address={0}&region={1}", Uri.EscapeDataString(address), region));
        }
    }
}
