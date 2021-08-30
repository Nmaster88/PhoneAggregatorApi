using Aggregator.BusinessSector;
using Aggregator.BusinessSector.Dtos;
using AggregatorAPI.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AggregatorAPI.Services
{
    public class AggregateService : IAggregateService
    {
        private Dictionary<string, Dictionary<string, int>> _phoneNumbersCountByPrefixAndBusinessSector { get; set; }

        private readonly IPrefixesService _prefixesService;
        private readonly IBusinessSectorService _businessSectorService;

        public AggregateService(IPrefixesService prefixesService, IBusinessSectorService businessSectorService)
        {
            _prefixesService = prefixesService;
            _businessSectorService = businessSectorService;
        }

        /// <summary>
        /// Get valid phone numbers count aggregated by prefix and business unit
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, Dictionary<string, int>>> GetValidPhoneNumbersCountAggregated(List<string> phoneNumbers)
        {
            _phoneNumbersCountByPrefixAndBusinessSector = new Dictionary<string, Dictionary<string, int>>();
            foreach (var phoneNumber in phoneNumbers)
            {
                await PhoneNumberCountByPrefixAndBusinessSector(phoneNumber);
            }
            return _phoneNumbersCountByPrefixAndBusinessSector;
        }

        /// <summary>
        ///  Add phone number to the aggregate dictionary of phone numbers count by prefix and business sector
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private async Task PhoneNumberCountByPrefixAndBusinessSector(string phoneNumber)
        {
            string prefix = AddAggregatePrefix(phoneNumber);
            await AddAggregateBusinessSector(phoneNumber, prefix);
        }

        /// <summary>
        ///  Add prefix to the aggregate dictionary of phone numbers count by prefix and business sector
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private string AddAggregatePrefix(string phoneNumber)
        {
            if(phoneNumber.StartsWith("00"))
            {
                phoneNumber = phoneNumber.Substring(2);
            }
            else if (phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.Substring(1);
            }

            string prefix = _prefixesService.GetPhonePrefixes().SingleOrDefault(p => phoneNumber.StartsWith(p));
            if (!_phoneNumbersCountByPrefixAndBusinessSector.Any(p => phoneNumber.StartsWith(p.Key)))
            {    
                _phoneNumbersCountByPrefixAndBusinessSector.Add(prefix, null);
            }

            return prefix;
        }

        /// <summary>
        /// Add business sector to the aggregate dictionary of phone numbers count by prefix and business sector
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private async Task AddAggregateBusinessSector(string phoneNumber, string prefix)
        {
            var prefixEntryValue = _phoneNumbersCountByPrefixAndBusinessSector.SingleOrDefault(p => p.Key == prefix).Value;

            BusinessSectorResponse response = await _businessSectorService.GetBusinessSector(phoneNumber);

            if(response != null)
            {
                string businessSector = response.sector;

                if (prefixEntryValue == null)
                {
                    Dictionary<string, int> entry = new Dictionary<string, int>();
                    entry.Add(businessSector, 1);
                    _phoneNumbersCountByPrefixAndBusinessSector[prefix] = entry;
                }
                else
                {
                    int countBusinessSector = _phoneNumbersCountByPrefixAndBusinessSector[prefix].SingleOrDefault(p => p.Key == businessSector).Value;
                    _phoneNumbersCountByPrefixAndBusinessSector[prefix][businessSector] = ++countBusinessSector;
                }
            }
        }
    }
}
