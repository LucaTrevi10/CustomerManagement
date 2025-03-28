using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace CustomerUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
        {
            _httpClient = clientFactory.CreateClient("LoginService");
            _localStorage = localStorage;
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/auth/login/{username}/{password}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Errore HTTP: {response.StatusCode}");
                    return false;
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result != null)
                {
                    await _localStorage.SetItemAsync("authToken", result.Token);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la richiesta: {ex.Message}");
            }

            return false;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
