using Aggregator.BusinessSector.Dtos;
using RestSharp;
using System;
using System.Web;
using System.Threading.Tasks;
using System.Linq;

namespace Aggregator.BusinessSector
{
    public interface IBusinessSectorService
    {
        Task<BusinessSectorResponse> GetBusinessSector(string phoneNumber);
    }
    public class BusinessSectorService : IBusinessSectorService
    {
        public BusinessSectorService()
        {

        }

        public async Task<BusinessSectorResponse> GetBusinessSector(string phoneNumber)
        {
            RestRequest request = CreateBaseRequest($"{Configuration.BaseUrl}/sector/{Uri.EscapeUriString(phoneNumber)}", Method.GET);
            request.AddJsonBody(phoneNumber);

            BusinessSectorClient client = new BusinessSectorClient();

            BusinessSectorResponse response = null; 
            try
            {
                response = await client.ExecuteAsync<BusinessSectorResponse>(request);
            }
            catch(Exception ex)
            {
                //TODO: Catch exception and log
            }

            return response;
        }

        private RestRequest CreateBaseRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.Parameters.Clear();
            request.RequestFormat = DataFormat.Json;
            request.Timeout = Configuration.Timeout;

            return request;
        }
    }
}
