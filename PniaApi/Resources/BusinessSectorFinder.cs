using Newtonsoft.Json;
using PniaApi.ExternalRequests;
using PniaApi.Models.Resources;

namespace PniaApi.Resources
{
    /// <summary>Gathers operations related with finding the business sector of phone numbers.</summary>
    public class BusinessSectorFinder
    {
        private readonly string _externalUrl;

        public BusinessSectorFinder(string externalUrl)
        {
            _externalUrl = externalUrl;
        }

        /// <summary>Gets the business sector of a phone number.</summary>
        /// <param name="phoneData">The phone data with its number.</param>
        /// <returns>Phone data with its correponding business sector.</returns>
        public PhoneData GetSectorByPhoneNumber(PhoneCoreData phoneCoreData)
        {
            var phoneResult = new PhoneData
            {
                Prefix = phoneCoreData.Prefix,
                Number = phoneCoreData.Number
            };

            // Service invocation
            var serviceInvoker = new ServiceInvoker(_externalUrl);
            var result = serviceInvoker.GetRequest(phoneCoreData.Number);

            if (result.Result?.IsSuccessStatusCode == true)
            {
                phoneResult.Sector = JsonConvert.DeserializeObject<PhoneData>(result.Result.ContentResult)?.Sector;
            }
            else
            {
                phoneResult.Sector = null;
            }
            return phoneResult;
        }
    }
}
