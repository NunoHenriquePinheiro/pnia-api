using PniaApi.Cache;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PniaApi.Resources
{
    /// <summary>Class to manage the listing of phone prefixes.</summary>
    public static class PrefixesLister
    {
        /// <summary>Loads the phone prefixes.</summary>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="prefixesFilepath">The prefixes file path.</param>
        /// <returns>List of phone prefixes.</returns>
        public static List<string> LoadPrefixes(CacheManager cacheManager, string prefixesFilepath)
        {
            // Use cache to avoid processing the prefixes file all the time
            if (!cacheManager.TryGetFromCache(CacheKeys.PhonePrefixes, out List<string> prefixes))
            {
                prefixes = LoadPrefixesFromFile(prefixesFilepath);
                cacheManager.SetToCache(CacheKeys.PhonePrefixes, prefixes);
            }
            return prefixes;
        }


        #region Private methods

        /// <summary>Loads the prefixes from file.</summary>
        /// <param name="prefixesFilename">The prefixes filename.</param>
        /// <returns>List of phone prefixes loaded from file.</returns>
        private static List<string> LoadPrefixesFromFile(string prefixesFilepath)
        {
            return File.ReadAllLines(prefixesFilepath).ToList();
        }

        #endregion
    }
}
