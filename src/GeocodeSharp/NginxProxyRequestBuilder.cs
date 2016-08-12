using System.Collections.Generic;
using System.Linq;

namespace GeocodeSharp
{
    public class NginxProxyRequestBuilder : DefaultRequestBuilder
    {
        public NginxProxyRequestBuilder(string domain, bool isProtected = true) : base(domain, isProtected)
        {
        }

        public NginxProxyRequestBuilder(string domain, string apiKey): this(domain)
        {
            _clientKey = apiKey;
        }

        public NginxProxyRequestBuilder(string domain, string clientId, string cryptoKey): this(domain)
        {
            _clientId = clientId;
            _cryptoKey = cryptoKey;
        }
    }
}
