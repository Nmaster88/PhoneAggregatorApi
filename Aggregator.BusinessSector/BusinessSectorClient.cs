using RestSharp;
using System;
using System.Threading.Tasks;

namespace Aggregator.BusinessSector
{
    public class BusinessSectorClient
    {
        private readonly RestClient restClient;

        private readonly string url;

        public BusinessSectorClient()
        {
            url = Configuration.BaseUrl;

            restClient = new RestClient();
            restClient.BaseUrl = new Uri(url);
        }

        public async Task<T> ExecuteAsync<T>(RestRequest request) where T : class
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            request.AddHeader("Content-Type", "application/json");

            IRestResponse<T> response = await restClient.ExecuteAsync<T>(request);   // << Making the actual request.

            if(response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Invalid number");
            }

            return response.Data;
        }
    }
}
