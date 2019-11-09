using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PniaApi.AppSettings;
using PniaApi.Cache;
using PniaApi.Operators;
using PniaApi.Operators.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PniaApi.Controllers
{
    /// <summary>Controller in the Pnia API to aggregate phone numbers by prefix and business sector.</summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("[controller]")]
    public class AggregateController : ControllerBase
    {
        private const string _prefixesFilename = "prefixes.txt";
        private readonly ServiceEndpoints _serviceEndpoints;
        private readonly CacheManager _cacheManager;


        public AggregateController(IMemoryCache memoryCache, IOptions<CacheOptions> cacheOptions, IOptions<ServiceEndpoints> serviceEndpoints)
        {
            _serviceEndpoints = serviceEndpoints.Value;

            if (!bool.TryParse(cacheOptions.Value.UseCache, out bool useCache))
            {
                useCache = true;
            }
            
            if (!double.TryParse(cacheOptions.Value.ExpireTimeMinutes, out double cacheExpireTime))
            {
                cacheExpireTime = 5; // in minutes, respecting the appsetting
            }

            _cacheManager = new CacheManager(memoryCache, useCache, cacheExpireTime);
        }


        /// <summary>Aggregates the specified phone numbers by prefix and business sector.</summary>
        /// <param name="phoneNumbers">The phone numbers.</param>
        /// <returns>Phone number prefixes and the business sectors that relate to them.</returns>
        [HttpPost]
        public IActionResult Aggregate([FromBody] List<string> phoneNumbers)
        {
            try
            {
                if (!phoneNumbers.Any())
                {
                    return NoContent();
                }

                var phoneDataList = PrefixValidator.GetValidPhoneNumbers(_cacheManager, _prefixesFilename, phoneNumbers);
                if (!phoneDataList.Any())
                {
                    return NoContent();
                }

                var phoneDataCompleteList = PhoneBusinessHandler.CompleteInfoPhoneData(phoneDataList, _serviceEndpoints.PhoneBusinessSector);
                if (!phoneDataCompleteList.Any())
                {
                    return NoContent();
                }

                var prefixBusinessCountList = PhoneBusinessHandler.GetBusinessCountsByPrefix(phoneDataCompleteList);
                return Ok(prefixBusinessCountList.ToDictionaryJsonString());
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}
