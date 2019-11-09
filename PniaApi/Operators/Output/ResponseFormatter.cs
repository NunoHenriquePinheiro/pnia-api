using Newtonsoft.Json;
using PniaApi.Models.Output;
using System.Collections.Generic;

namespace PniaApi.Operators.Output
{
    /// <summary>Gathers methods to format the response to be retrieved by the service.</summary>
    public static class ResponseFormatter
    {
        /// <summary>Converts to a JSON string with a mapped dictionary.</summary>
        /// <param name="prefixBizList">The prefix-business relations list.</param>
        /// <returns>JSON string with a dictionary corresponding to the list of prefix-business relations.</returns>
        public static string ToDictionaryJsonString(this IEnumerable<PrefixBusinessCounts> prefixBizList)
        {
            return JsonConvert.SerializeObject(prefixBizList.ToDictionary());
        }


        #region Private methods

        /// <summary>Converts a business count list to a dictionary.</summary>
        /// <param name="bizCountList">The business count list.</param>
        /// <returns>Dictionary corresponding to the business count list.</returns>
        private static Dictionary<string, int> ToDictionary(this IEnumerable<BusinessCount> bizCountList)
        {
            var output = new Dictionary<string, int>();
            foreach (var bizCount in bizCountList)
            {
                output.Add(bizCount.Name, bizCount.Quantity);
            }
            return output;
        }

        /// <summary>Converts a prefix-business relations list to a dictionary.</summary>
        /// <param name="prefixBizList">The prefix-business relations list.</param>
        /// <returns>Dictionary corresponding to the prefix-business relations list.</returns>
        private static Dictionary<string, Dictionary<string, int>> ToDictionary(this IEnumerable<PrefixBusinessCounts> prefixBizList)
        {
            var output = new Dictionary<string, Dictionary<string, int>>();
            foreach (var prefixBiz in prefixBizList)
            {
                var valueToAdd = prefixBiz.BusinessCountList.ToDictionary();
                output.Add(prefixBiz.Prefix, valueToAdd);
            }
            return output;
        }

        #endregion
    }
}
