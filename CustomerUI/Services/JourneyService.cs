using Blazored.LocalStorage;
using CustomerUI.Models.Dtos;
using System.Net.Http.Json;

namespace CustomerUI.Services
{
    public class JourneyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public JourneyService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
        {
            // Utilizziamo lo stesso client configurato per il CustomerService, oppure puoi specificarne uno nuovo se preferisci
            _httpClient = clientFactory.CreateClient("CustomerService");
            _localStorage = localStorage;
        }

        // Recupera tutti gli step del journey per un determinato customer
        public async Task<List<JourneyStepDto>> GetJourneyStepsAsync(int customerId)
        {
            try
            {
                return await _httpClient
                    .GetFromJsonAsync<List<JourneyStepDto>>($"api/journey/customer/{customerId}")
                    ?? new List<JourneyStepDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetJourneyStepsAsync - exception: " + ex.ToString());
                return new List<JourneyStepDto>();
            }
        }

        // Aggiunge un nuovo step al journey per il customer specificato
        public async Task AddJourneyStepAsync(int customerId, JourneyStepDto journeyStep)
        {
            var response = await _httpClient
                .PostAsJsonAsync($"api/journey/customer/{customerId}", journeyStep);
            response.EnsureSuccessStatusCode();
        }

        // Aggiorna uno step esistente
        public async Task UpdateJourneyStepAsync(int journeyStepId, JourneyStepDto journeyStep)
        {
            var response = await _httpClient
                .PutAsJsonAsync($"api/journey/{journeyStepId}", journeyStep);
            response.EnsureSuccessStatusCode();
        }

        // Elimina uno step esistente
        public async Task DeleteJourneyStepAsync(int journeyStepId)
        {
            var response = await _httpClient
                .DeleteAsync($"api/journey/{journeyStepId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
