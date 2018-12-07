using System;
using System.Collections.Generic;
using System.Linq;

namespace GeocodeSharp.Google
{
    public class LocationTypeFilter
    {
        private Dictionary<string, bool> parameters = new Dictionary<string, bool>();

        /// <summary>
        ///  Indicates a precise street address.
        /// </summary>
        public bool Rooftop
        {
            get { return GetParameterIfExists("ROOFTOP"); }
            set { parameters["ROOFTOP"] = value; }
        }

        /// <summary>
        /// Indicates a named route (such as "US 101").
        /// </summary>
        public bool RangeInterpolated
        {
            get { return GetParameterIfExists("RANGE_INTERPOLATED"); }
            set { parameters["RANGE_INTERPOLATED"] = value; }
        }

        /// <summary>
        /// Indicates a major intersection, usually of two major roads.
        /// </summary>
        public bool GeometricCenter
        {
            get { return GetParameterIfExists("GEOMETRIC_CENTER"); }
            set { parameters["GEOMETRIC_CENTER"] = value; }
        }

        /// <summary>
        /// Indicates a political entity. Usually, this type indicates a polygon of some civil administration.
        /// </summary>
        public bool Approximate
        {
            get { return GetParameterIfExists("APPROXIMATE"); }
            set { parameters["APPROXIMATE"] = value; }
        }

        /// <summary>
        /// Converts filter to Url string.
        /// </summary>
        internal string ToUrlParameters()
        {
            var parametersList = parameters.Where(p => p.Value).Select(p => p.Key);
            return Uri.EscapeDataString(string.Join("|", parametersList));
        }

        private bool GetParameterIfExists(string key)
        {
            return parameters.ContainsKey(key) && parameters[key];
        }
    }
}
