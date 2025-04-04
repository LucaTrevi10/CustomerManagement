using CustomerUI.Models;
using CustomerUI.Models.Dtos;
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

        public async Task<List<CustomerDto>> GetCustomersFromSalesPipelineAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CustomerDto>>("api/SalesPipeline/customers")
                       ?? new List<CustomerDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetCustomersFromSalesPipelineAsync - exception: " + ex);
                return new();
            }
        }

        public async Task UpdateSalesPipelineNoteAsync(int pipelineId, string newNote)
        {
            var dto = new { Note = newNote };
            var response = await _httpClient.PutAsJsonAsync($"api/SalesPipeline/{pipelineId}/notes", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateSalesPipelineCurrentStageAsync(int pipelineId, string newStage)
        {
            var dto = new { CurrentStage = newStage };
            var response = await _httpClient.PutAsJsonAsync($"api/SalesPipeline/{pipelineId}/currentstage", dto);
            response.EnsureSuccessStatusCode();
        }

    }
}
