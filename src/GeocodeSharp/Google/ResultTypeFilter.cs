using System;
using System.Collections.Generic;
using System.Linq;

namespace GeocodeSharp.Google
{
    public class ResultTypeFilter
    {
        private Dictionary<string, bool> parameters = new Dictionary<string, bool>();

        /// <summary>
        ///  Indicates a precise street address.
        /// </summary>
        public bool StreetAddress
        {
            get { return GetParameterIfExists("street_address"); }
            set { parameters["street_address"] = value; }
        }

        /// <summary>
        /// Indicates a named route (such as "US 101").
        /// </summary>
        public bool Route
        {
            get { return GetParameterIfExists("route"); }
            set { parameters["route"] = value; }
        }

        /// <summary>
        /// Indicates a major intersection, usually of two major roads.
        /// </summary>
        public bool Intersection
        {
            get { return GetParameterIfExists("intersection"); }
            set { parameters["intersection"] = value; }
        }

        /// <summary>
        /// Indicates a political entity. Usually, this type indicates a polygon of some civil administration.
        /// </summary>
        public bool Political
        {
            get { return GetParameterIfExists("political"); }
            set { parameters["political"] = value; }
        }

        /// <summary>
        /// Indicates the national political entity, and is typically the highest order type returned by the Geocoder.
        /// </summary>
        public bool Country
        {
            get { return GetParameterIfExists("country"); }
            set { parameters["country"] = value; }
        }

        /// <summary>
        /// Indicates a first-order civil entity below the country level. Within the United States, these administrative levels are states. 
        /// Not all nations exhibit these administrative levels. 
        /// In most cases, administrative_area_level_1 short names will closely match ISO 3166-2 subdivisions and other widely circulated lists; 
        /// however this is not guaranteed as our geocoding results are based on a variety of signals and location data.
        /// </summary>
        public bool AdministrativeAreaLevel1
        {
            get { return GetParameterIfExists("administrative_area_level_1"); }
            set { parameters["administrative_area_level_1"] = value; }
        }

        /// <summary>
        /// Indicates a second-order civil entity below the country level. 
        /// Within the United States, these administrative levels are counties. Not all nations exhibit these administrative levels.
        /// </summary>
        public bool AdministrativeAreaLevel2
        {
            get { return GetParameterIfExists("administrative_area_level_2"); }
            set { parameters["administrative_area_level_2"] = value; }
        }

        /// <summary>
        /// Indicates a third-order civil entity below the country level. 
        /// This type indicates a minor civil division. Not all nations exhibit these administrative levels.
        /// </summary>
        public bool AdministrativeAreaLevel3
        {
            get { return GetParameterIfExists("administrative_area_level_3"); }
            set { parameters["administrative_area_level_3"] = value; }
        }

        /// <summary>
        /// Indicates a fourth-order civil entity below the country level. 
        /// This type indicates a minor civil division. Not all nations exhibit these administrative levels.
        /// </summary>
        public bool AdministrativeAreaLevel4
        {
            get { return GetParameterIfExists("administrative_area_level_4"); }
            set { parameters["administrative_area_level_4"] = value; }
        }

        /// <summary>
        /// Indicates a fifth-order civil entity below the country level. 
        /// This type indicates a minor civil division. Not all nations exhibit these administrative levels.
        /// </summary>
        public bool AdministrativeAreaLevel5
        {
            get { return GetParameterIfExists("administrative_area_level_5"); }
            set { parameters["administrative_area_level_5"] = value; }
        }

        /// <summary>
        /// Indicates a commonly-used alternative name for the entity.
        /// </summary>
        public bool ColloquialArea
        {
            get { return GetParameterIfExists("colloquial_area"); }
            set { parameters["colloquial_area"] = value; }
        }

        /// <summary>
        /// Indicates an incorporated city or town political entity.
        /// </summary>
        public bool Locality
        {
            get { return GetParameterIfExists("locality"); }
            set { parameters["locality"] = value; }
        }

        /// <summary>
        /// Indicates a first-order civil entity below a locality. 
        /// </summary>
        public bool Sublocality
        {
            get { return GetParameterIfExists("sublocality"); }
            set { parameters["sublocality"] = value; }
        }

        /// <summary>
        /// Indicates a named neighborhood
        /// </summary>
        public bool Neighborhood
        {
            get { return GetParameterIfExists("neighborhood"); }
            set { parameters["neighborhood"] = value; }
        }

        /// <summary>
        /// Indicates a named location, usually a building or collection of buildings with a common name.
        /// </summary>
        public bool Premise
        {
            get { return GetParameterIfExists("premise"); }
            set { parameters["premise"] = value; }
        }

        /// <summary>
        /// Indicates a first-order entity below a named location, usually a singular building within a collection of buildings with a common name.
        /// </summary>
        public bool Subpremise
        {
            get { return GetParameterIfExists("subpremise"); }
            set { parameters["subpremise"] = value; }
        }

        /// <summary>
        /// Indicates a postal code as used to address postal mail within the country.
        /// </summary>
        public bool PostalCode
        {
            get { return GetParameterIfExists("postal_code"); }
            set { parameters["postal_code"] = value; }
        }

        /// <summary>
        /// Indicates a prominent natural feature.
        /// </summary>
        public bool NaturalFeature
        {
            get { return GetParameterIfExists("natural_feature"); }
            set { parameters["natural_feature"] = value; }
        }

        /// <summary>
        /// Indicates an airport.
        /// </summary>
        public bool Airport
        {
            get { return GetParameterIfExists("airport"); }
            set { parameters["airport"] = value; }
        }

        /// <summary>
        /// Indicates a named park.
        /// </summary>
        public bool Park
        {
            get { return GetParameterIfExists("park"); }
            set { parameters["park"] = value; }
        }

        /// <summary>
        /// Indicates a named point of interest. 
        /// Typically, these "POI"s are prominent local entities that don't easily fit in another category, such as "Empire State Building" or "Statue of Liberty."
        /// </summary>
        public bool PointOfInterest
        {
            get { return GetParameterIfExists("point_of_interest"); }
            set { parameters["point_of_interest"] = value; }
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
