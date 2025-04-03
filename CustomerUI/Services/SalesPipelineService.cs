using CustomerUI.Models;
using System.Net.Http.Json;

namespace CustomerUI.Services
{
    public class SalesPipelineService
    {
        private readonly HttpClient _httpClient;

        public SalesPipelineService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("SaleService");
        }

        public async Task<List<SalesPipeline>> GetSalesPipelineAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<SalesPipeline>>("api/SalesPipeline")
                   ?? new List<SalesPipeline>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSalesPipelineAsync - exception: " + ex.ToString());
                return new();
            }
        }
    }
}
