using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using GeocodeSharp.Google;

namespace GeocodeSharp
{
    public class DefaultRequestBuilder: RequestBuilderBase
    {
        protected DefaultRequestBuilder(string domain, bool isProtected = true) : base(domain, isProtected)
        {
        }

        public DefaultRequestBuilder()
        {
        }

        public DefaultRequestBuilder(string apiKey): base(apiKey)
        {
        }

        public DefaultRequestBuilder(string clientId, string cryptoKey): base(clientId, cryptoKey)
        {
        }

        public override HttpWebRequest Build(ComponentFilter filter, string region)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");
            var addressPortion = BuildAddressPortion(filter, region);
            var authPortion = BuildAuthPortion(addressPortion);
            return HttpWebRequest.CreateHttp(ConcatUrlParts(addressPortion, authPortion));
        }

        string ConcatUrlParts(string addressPortion, string authPortion)
        {
            return string.Format("{0}{1}{2}{3}", _domain, _apiPath, addressPortion, authPortion);
        }

        public override HttpWebRequest Build(ComponentFilter filter, string region, string address, string language)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException("address");
            var addressPortion = BuildAddressPortion(address, region, language, filter);
            var authPortion = BuildAuthPortion(addressPortion);
            return HttpWebRequest.CreateHttp(ConcatUrlParts(addressPortion, authPortion));
        }

        private string BuildAuthPortion(string addressPortion)
        {
            switch (_mode)
            {
                case UsageMode.Free:
                    return string.Empty;
                case UsageMode.ClientKey:
                    return string.Format("&key={0}", _clientKey);
                case UsageMode.ApiForWork:
                    return BuildApiForWorkUrl(addressPortion);
                default:
                    return string.Empty;
            }
        }

        private string BuildAddressPortion(ComponentFilter filter, string region)
        {
            var filterString = filter.ToUrlParameters();
            if (string.IsNullOrWhiteSpace(filterString))
                throw new ArgumentException("Component filter doesn't contain any component", "filter");
            var addressPortion = string.Format("components={0}", filterString);
            if (!string.IsNullOrWhiteSpace(region))
            {
                addressPortion += string.Format("&region={0}", Uri.EscapeDataString(region));
            }

            return addressPortion;
        }

        private string BuildAddressPortion(string address, string region, string language, ComponentFilter filter)
        {
            var addressPortion = string.Format("address={0}", Uri.EscapeDataString(address));
            if (!string.IsNullOrWhiteSpace(region))
            {
                addressPortion += string.Format("&region={0}", Uri.EscapeDataString(region));
            }

            if (!string.IsNullOrWhiteSpace(language))
            {
                addressPortion += string.Format("&language={0}", Uri.EscapeDataString(language));
            }

            if (filter != null)
            {
                var filterString = filter.ToUrlParameters();
                if (!string.IsNullOrWhiteSpace(filterString))
                {
                    addressPortion += string.Format("&components={0}", filterString);
                }
            }

            return addressPortion;
        }

        private string BuildApiForWorkUrl(string addressPortion)
        {
            var cryptoBytes = Convert.FromBase64String(_cryptoKey.Replace("-", "+").Replace("_", "/"));
            var hashThis = string.Format("{0}{1}&client={2}", _apiPath, addressPortion, _clientId);
            var hashThisBytes = Encoding.ASCII.GetBytes(hashThis);
            using (var sha1 = new HMACSHA1(cryptoBytes))
            {
                var hash = sha1.ComputeHash(hashThisBytes);
                var signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");
                return string.Format("&client={0}&signature={1}", _clientId, signature);
            }
        }
    }
}