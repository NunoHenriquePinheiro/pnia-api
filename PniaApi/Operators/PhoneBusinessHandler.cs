using PniaApi.Models.Output;
using PniaApi.Models.Resources;
using PniaApi.Resources;
using System.Collections.Generic;
using System.Linq;

namespace PniaApi.Operators
{
    /// <summary>Gathers methods to handle the relationship between a phone number, its prefix and its business sector.</summary>
    public static class PhoneBusinessHandler
    {
        /// <summary>Completes the information on phone data.</summary>
        /// <param name="phoneDataList">The phone data list.</param>
        /// <returns>List with complete phone data (prefix, number and business sector).</returns>
        public static List<PhoneData> CompleteInfoPhoneData(List<PhoneCoreData> phoneDataList, string externalUrl)
        {
            var bizSectorFinder = new BusinessSectorFinder(externalUrl);

            var completePhoneDataList = new List<PhoneData>();
            foreach (var phoneData in phoneDataList)
            {
                // Get business sector for each phone number
                var completePhoneData = bizSectorFinder.GetSectorByPhoneNumber(phoneData);

                if (completePhoneData.Sector != null)
                {
                    completePhoneDataList.Add(completePhoneData);
                }
            }
            return completePhoneDataList;
        }

        /// <summary>Gets the business sector counts by each prefix.</summary>
        /// <param name="phoneDataList">The phone data list (with prefix, number and business sector).</param>
        /// <returns>List of prefixes, with the count of business sectors for each of them.</returns>
        public static List<PrefixBusinessCounts> GetBusinessCountsByPrefix(List<PhoneData> phoneDataList)
        {
            var prefixList = phoneDataList.Select(x => x.Prefix).Distinct();
            var sectorList = phoneDataList.Select(x => x.Sector).Distinct();

            var prefixBizCountsList = new List<PrefixBusinessCounts>();
            foreach (var prefix in prefixList)
            {
                var prefixBizCounts = new PrefixBusinessCounts
                {
                    Prefix = prefix,
                    BusinessCountList = new List<BusinessCount>()
                };

                foreach (var sector in sectorList)
                {
                    // Check for existence of combination prefix+sector, before counting, for speed
                    if (phoneDataList.Any(x => x.Prefix.Equals(prefix) && x.Sector.Equals(sector)))
                    {
                        prefixBizCounts.BusinessCountList.Add(
                            new BusinessCount
                            {
                                Name = sector,
                                Quantity = phoneDataList.Count(x => x.Prefix.Equals(prefix) && x.Sector.Equals(sector))
                            });
                    }
                }

                prefixBizCountsList.Add(prefixBizCounts);
            }

            return prefixBizCountsList;
        }
    }
}
