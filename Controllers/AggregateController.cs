using AggregatorAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AggregatorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AggregateController : ControllerBase
    {
        private readonly ILogger<AggregateController> _logger;
        private readonly IAggregateService _aggregateService;

        public AggregateController(ILogger<AggregateController> logger, IAggregateService aggregateService)
        {
            _logger = logger;
            _aggregateService = aggregateService;
        }


        /// <summary>
        /// Get valid phone numbers count by prefix and business sector
        /// </summary>
        /// <param name="phoneNumbers">list of phone numbers</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Dictionary<string, Dictionary<string, int>>> Post([FromBody] List<string> phoneNumbers)
        {
            try
            {
                return await _aggregateService.GetValidPhoneNumbersCountAggregated(phoneNumbers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An exception occurred on endpoint POST /Aggregate with the message ${ex.Message}");
                return null;
            }
        }
    }
}
