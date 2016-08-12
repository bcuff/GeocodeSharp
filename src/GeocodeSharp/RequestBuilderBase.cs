using System.Net;
using GeocodeSharp.Google;

namespace GeocodeSharp
{
    public abstract class RequestBuilderBase
    {
        protected string _clientKey;
        protected UsageMode _mode;
        protected string _clientId;
        protected string _cryptoKey;

        protected string _domain = "https://maps.googleapis.com";
        protected const string _apiPath = "/maps/api/geocode/json?";

        /// <param name="domain"></param>
        /// <param name="isProtected">only needed so that signature doesnt collide with another ctor</param>
        protected RequestBuilderBase(string domain, bool isProtected = true)
        {
            _domain = domain;
        }

        /// <summary>
        /// Dont use a Google API key and use default anonymous access.
        /// NOTE: Throttling may apply.
        /// </summary>
        public RequestBuilderBase()
        {
            _mode = UsageMode.Free;
        }

        /// <summary>
        /// Initialize GeocodeClient with your Google API key to utilize it in the requests to Google and bypass the default annonymous throttling.
        /// </summary>
        /// <param name="apiKey">Google Maps API Key</param>
        public RequestBuilderBase(string apiKey)
        {
            _clientKey = apiKey;
            _mode = UsageMode.ClientKey;
        }

        /// <summary>
        /// Initialize GeocodeClient with your Google API key to utilize it in the requests to Google and bypass the default annonymous throttling.
        /// </summary>
        /// <param name="clientId">The client ID. Applicable when using Maps API for Work.</param>
        /// <param name="cryptoKey">The base64 encoded crypto key. Applicable when using Maps API for Work.</param>
        /// <remarks>
        /// See - https://developers.google.com/maps/documentation/business/webservices/#client_id
        /// </remarks>
        public RequestBuilderBase(string clientId, string cryptoKey)
        {
            _clientId = clientId;
            _cryptoKey = cryptoKey;
            _mode = UsageMode.ApiForWork;
        }

        public abstract HttpWebRequest Build(ComponentFilter filter, string region);
        public abstract HttpWebRequest Build(ComponentFilter filter, string region, string address, string language);
    }
}