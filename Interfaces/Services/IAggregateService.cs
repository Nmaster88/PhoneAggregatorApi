using System.Collections.Generic;
using System.Threading.Tasks;

namespace AggregatorAPI.Interfaces.Services
{
    public interface IAggregateService
    {
        /// <summary>
        /// Get valid phone numbers count aggregated by prefix and business unit
        /// </summary>
        /// <param name="phoneNumbers"></param>
        /// <returns>A dictionary structure of valid phone numbers count aggregated by prefix and business unit</returns>
        Task<Dictionary<string, Dictionary<string, int>>> GetValidPhoneNumbersCountAggregated(List<string> phoneNumbers);
    }
}
