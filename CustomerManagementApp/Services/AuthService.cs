using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CustomerManagementApp.Models.Login;
using Microsoft.AspNetCore.Components;

namespace CustomerManagementApp.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Username = username, Password = password });
            return response.IsSuccessStatusCode;
        }

        public async Task Logout()
        {
            await _httpClient.PostAsync("auth/logout", null);
        }
    }
}
