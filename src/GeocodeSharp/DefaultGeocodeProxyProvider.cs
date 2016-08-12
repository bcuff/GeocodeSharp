using System.Net;

namespace GeocodeSharp
{
    public class DefaultGeocodeProxyProvider: IGeocodeProxyProvider
    {
        public HttpWebRequest CreateRequest(string url)
        {
            return WebRequest.CreateHttp(url);
        }
    }
}