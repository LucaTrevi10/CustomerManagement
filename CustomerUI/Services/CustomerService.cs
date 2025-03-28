using Blazored.LocalStorage;
using CustomerUI.Models;
using System.Net.Http.Json;

namespace CustomerUI.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CustomerService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
        {
            _httpClient = clientFactory.CreateClient("CustomerService");
            _localStorage = localStorage;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Customer>>("api/customer")
                   ?? new List<Customer>();
            }
            catch(Exception ex)
            {
                Console.WriteLine("GetCustomersAsync - exception: " + ex.ToString());
                return new();
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Customer>($"api/customer/{id}")
                   ?? throw new Exception("Customer not found");
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customer", customer);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateCustomerAsync(int id, Customer customer)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/customer/{id}", customer);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/customer/{id}");
            response.EnsureSuccessStatusCode();
        }

    }
}
