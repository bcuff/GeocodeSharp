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

        /// <summary>
        /// Initialize GeocodeClient without a Google API key and use default annonymouse access.
        /// NOTE: Throttling may apply.
        /// </summary>
        public GeocodeClient()
        {

        }

        /// <summary>
        /// Initialize GeocodeClient with your Google API key to utilize it in the requests to Google and bypass the default annonymous throttling.
        /// </summary>
        /// <param name="apiKey">Google Maps API Key</param>
        public GeocodeClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<GeocodeResponse> GeocodeAddress(string address, bool sensor = false)
        {
            if (address == null) throw new ArgumentNullException("address");

            const string urlFormat = "{0}://maps.googleapis.com/maps/api/geocode/json?address={1}{2}";

            var url = !string.IsNullOrEmpty(_apiKey)
                ? string.Format(urlFormat, "https", Uri.EscapeDataString(address), "&key=" + Uri.EscapeDataString(_apiKey))
                : string.Format(urlFormat, "http", Uri.EscapeDataString(address), string.Empty);

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
    }
}
