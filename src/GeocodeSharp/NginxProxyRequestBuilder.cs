using System.Collections.Generic;
using System.Linq;

namespace GeocodeSharp
{
    public class NginxProxyRequestBuilder : DefaultRequestBuilder
    {
        public NginxProxyRequestBuilder(string domain, bool isProtected = true) : base(domain, isProtected)
        {
        }

        public NginxProxyRequestBuilder(string apiKey) : base(apiKey)
        {
        }

        public NginxProxyRequestBuilder(string clientId, string cryptoKey) : base(clientId, cryptoKey)
        {
        }
    }
}
