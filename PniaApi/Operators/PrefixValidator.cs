using PniaApi.Cache;
using PniaApi.Models.Resources;
using PniaApi.Resources;
using System.Collections.Generic;
using System.Linq;

namespace PniaApi.Operators
{
    /// <summary>Gathers methods to validate phone numbers according to business rules.</summary>
    public static class PrefixValidator
    {
        private const string _defaultPrefixesFilepath = "prefixes.txt";
        private const string _plusSign = "+";
        private const string _doubleZeros = "00";


        /// <summary>Gets info about the valid phone numbers, removing the invalid ones.</summary>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="prefixesFilepath">The prefixes file path.</param>
        /// <param name="phoneNumbers">The phone numbers.</param>
        /// <returns>List of valid phone numbers.</returns>
        public static List<PhoneCoreData> GetValidPhoneNumbers(CacheManager cacheManager, string prefixesFilepath, List<string> phoneNumbers)
        {
            prefixesFilepath = string.IsNullOrWhiteSpace(prefixesFilepath) ? _defaultPrefixesFilepath : prefixesFilepath;

            var rawNumbers = FilterNonNumbers(GetRawNumbers(phoneNumbers));
            if (rawNumbers.Any())
            {
                var prefixList = PrefixesLister.LoadPrefixes(cacheManager, prefixesFilepath);
                return GetNumbersWithPrefix(rawNumbers, prefixList);
            }
            return new List<PhoneCoreData>();
        }


        #region Private methods

        /// <summary>Gets the raw numbers, without a plus signal ("+") or double zeros ("00") in the beginning.</summary>
        /// <param name="phoneNumbers">The phone numbers.</param>
        /// <returns>List of raw phone numbers.</returns>
        private static List<string> GetRawNumbers(List<string> phoneNumbers)
        {
            var noPlusList = phoneNumbers.Where(x => x.StartsWith(_plusSign)).Select(x => x.Remove(0, 1));
            var noZerosList = phoneNumbers.Where(x => x.StartsWith(_doubleZeros)).Select(x => x.Remove(0, 2));
            var noIndicationList = phoneNumbers.Where(x => !x.StartsWith(_doubleZeros) && !x.StartsWith(_plusSign));

            var rawNumbers = new List<string>();
            rawNumbers.AddRange(noPlusList);
            rawNumbers.AddRange(noZerosList);
            rawNumbers.AddRange(noIndicationList);
            return rawNumbers;
        }

        /// <summary>Filters the non numbers.</summary>
        /// <param name="rawNumbers">The raw numbers.</param>
        /// <returns>List of valid phone numbers.</returns>
        private static List<string> FilterNonNumbers(List<string> rawNumbers)
        {
            return rawNumbers.Where(x => x.Replace(" ", "").All(char.IsDigit)).ToList();
        }

        /// <summary>Gets the phone numbers that have a correspondence in the prefixes list.</summary>
        /// <param name="rawNumbers">The raw phone numbers.</param>
        /// <param name="prefixList">The prefixes list.</param>
        /// <returns>List of phone numbers with a corresponding prefix in the prefixes list.</returns>
        private static List<PhoneCoreData> GetNumbersWithPrefix(List<string> rawNumbers, List<string> prefixList)
        {
            var validNumbers = new List<PhoneCoreData>();
            foreach (var number in rawNumbers)
            {
                var prefix = prefixList.FirstOrDefault(x => number.StartsWith(x));
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    validNumbers.Add(
                        new PhoneCoreData
                        {
                            Prefix = prefix,
                            Number = number
                        });
                }
            }
            return validNumbers;
        }

        #endregion
    }
}
