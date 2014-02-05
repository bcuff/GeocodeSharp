using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GeocodeSharp.Google
{
    public class GeocodeClient
    {
        public async Task<GeocodeResponse> GeocodeAddress(string address, bool sensor)
        {
            if (address == null) throw new ArgumentNullException("address");

            var url = string.Format(
                "http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor={1}",
                Uri.EscapeDataString(address),
                sensor.ToString().ToLower());
            string json;
            var request = WebRequest.CreateHttp(url);
            using (var ms = new MemoryStream())
            {
                using (var response = await request.GetResponseAsync())
                using (var body = response.GetResponseStream())
                {
                    await body.CopyToAsync(ms);
                }

                json = Encoding.UTF8.GetString(ms.ToArray());
            }
            return JsonConvert.DeserializeObject<GeocodeResponse>(json);
        }
    }
}
