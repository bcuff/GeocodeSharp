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

            var url = string.Format(
                "http://maps.googleapis.com/maps/api/geocode/json?address={0}",
                Uri.EscapeDataString(address));

            if (!string.IsNullOrEmpty(_apiKey))
            {
                url = string.Format(url + "&key={0}", _apiKey);

                // If an API key is specified we have to request via SSL.
                url = url.Replace("http", "https");
            }

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
