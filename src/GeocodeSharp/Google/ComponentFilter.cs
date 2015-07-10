using System;
using System.Collections.Generic;
using System.Linq;

namespace GeocodeSharp.Google
{
    /// <summary>
    /// A component filter for which you wish to obtain a geocode.
    /// </summary>
    public class ComponentFilter
    {
        private Dictionary<string, string> parameters = new Dictionary<string, string>();

        /// <summary>
        /// Matches long or short name of a route. 
        /// </summary>
        public string Route 
        {
            get { return TryGetParameter("route"); }
            set { parameters["route"] = value; } 
        }

        /// <summary>
        /// Matches against both locality and sublocality types.
        /// </summary>
        public string Locality
        {
            get { return TryGetParameter("locality"); }
            set { parameters["locality"] = value; }
        }

        /// <summary>
        /// Matches all the administrative_area levels.
        /// </summary>
        public string AdministrativeArea
        {
            get { return TryGetParameter("administrative_area"); }
            set { parameters["administrative_area"] = value; }
        }

        /// <summary>
        /// Matches postal_code and postal_code_prefix.
        /// </summary>
        public string PostalCode
        {
            get { return TryGetParameter("postal_code"); }
            set { parameters["postal_code"] = value; }
        }

        /// <summary>
        /// Matches a country name or a two letter ISO 3166-1 country code.
        /// </summary>
        public string Country
        {
            get { return TryGetParameter("country"); }
            set { parameters["country"] = value; }
        }

        /// <summary>
        /// Converts filter to Url string.
        /// </summary>
        internal string ToUrlParameters()
        {
            var parametersList = parameters.Where(p => !string.IsNullOrWhiteSpace(p.Value)).Select(p => p.Key + ":" + p.Value);
            return Uri.EscapeDataString(string.Join("|", parametersList));
        }

        private string TryGetParameter(string key)
        {
            string route = null;
            parameters.TryGetValue(key, out route);
            return route;
        }
    }
}
