using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MD.Tools.Helpers.Core.TypeConversion
{

    public static partial class Utility
    {

        private static IDictionary<string, CultureInfo> _mappedCultureInfo;

        /// <summary>
        /// Gets the mapped culture info.
        /// </summary>
        /// <value>The mapped culture info.</value>
        private static IDictionary<string, CultureInfo> MappedCultureInfo
        {
            get
            {
                if (_mappedCultureInfo == null)
                {
                    _mappedCultureInfo = new Dictionary<string, CultureInfo>();
                    System.Collections.Specialized.NameValueCollection nvc = Properties.HelperSettings.Default.CultureInfoLCIDMappings.ToNameValueCollection();
                    foreach (string key in nvc.Keys)
                    {
                        _mappedCultureInfo[key] = nvc[key].ToCultureInfo();
                    }
                }
                return _mappedCultureInfo;
            }
            
        }

        /// <summary>
        /// Resolves the culture for currency.
        /// </summary>
        /// <param name="currencyCode">The currency ISO code.</param>
        /// <returns>The first Culture Info object that uses the given currency</returns>
        public static CultureInfo ToCultureInfoForCurrency(this string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode)) throw new ArgumentNullException(nameof(currencyCode));
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo ri = new RegionInfo(ci.LCID);
                if (string.Equals(currencyCode, ri.ISOCurrencySymbol, StringComparison.OrdinalIgnoreCase)) return ci;
            }
            throw new NotSupportedException("There is no CultureInfo available for '{0}'".ToFormattedString( currencyCode));
        }

        /// <summary>
        /// Toes the culture info.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>
        /// The culture object represented by the langauge code
        /// </returns>
        public static CultureInfo ToCultureInfo(this string languageCode)
        {
            int localeLcid = languageCode.ToInt(-1, CultureInfo.InvariantCulture);
            if (localeLcid != -1) return new CultureInfo(localeLcid);
            if (MappedCultureInfo.ContainsKey(languageCode)) return MappedCultureInfo[languageCode];
            return new CultureInfo(languageCode);
        }


    }
}
