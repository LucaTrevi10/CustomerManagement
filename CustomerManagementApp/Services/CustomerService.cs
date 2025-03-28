using System.Net.Http.Json;
using CustomerManagementApp.Models;

namespace CustomerManagementApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Modifica del metodo per usare CustomerDto
        public async Task<List<Customer>> GetCustomers()
        {
            List<Customer> customers = null;

            try
            {
                customers = await _httpClient.GetFromJsonAsync<List<Customer>>("api/Customer");
                return customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Customer>();
            }
        }

        public async Task<Customer> GetCustomer(int id)
        {
            try
            {
                // Chiamata all'API per ottenere un cliente specifico
                return await _httpClient.GetFromJsonAsync<Customer>($"http://localhost:5202/api/Customer/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer with ID {id}: {ex.Message}");
                return null; // Restituisce null in caso di errore
            }
        }

        // Metodo per aggiungere un nuovo cliente
        public async Task AddCustomer(Customer customer)
        {
            try
            {
                // Chiamata all'API per aggiungere un nuovo cliente
                await _httpClient.PostAsJsonAsync("http://localhost:5202/api/Customer", customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
            }
        }

        // Metodo per aggiornare un cliente esistente
        public async Task UpdateCustomer(int id, Customer customer)
        {
            try
            {
                // Chiamata all'API per aggiornare il cliente
                await _httpClient.PutAsJsonAsync($"http://localhost:5202/api/Customer/{id}", customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating customer with ID {id}: {ex.Message}");
            }
        }

        // Metodo per eliminare un cliente
        public async Task DeleteCustomer(int id)
        {
            try
            {
                // Chiamata all'API per eliminare il cliente
                await _httpClient.DeleteAsync($"http://localhost:5202/api/Customer/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer with ID {id}: {ex.Message}");
            }
        }

        // Aggiungi un tag al cliente
        public async Task AddTagToCustomer(int customerId, string tagName)
        {
            try
            {
                // Creazione dell'oggetto Tag con il nome passato
                var tag = new { Name = tagName };

                // Chiamata all'API per aggiungere un tag
                var response = await _httpClient.PostAsJsonAsync($"http://localhost:5202/api/Customer/{customerId}/tags", tag);

                // Verifica la risposta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Errore nell'aggiungere il tag: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding tag: {ex.Message}");
            }
        }

        // Rimuovi un tag dal cliente
        public async Task RemoveTagFromCustomer(int customerId, string name)
        {
            try
            {
                // Chiamata all'API per rimuovere un tag
                var response = await _httpClient.DeleteAsync($"http://localhost:5202/api/Customer/{customerId}/tags/{name}");

                // Verifica la risposta
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Errore nella rimozione del tag: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing tag: {ex.Message}");
            }
        }

        // Ottieni i tag di un cliente
        public async Task<List<string>> GetCustomerTags(int customerId)
        {
            try
            {
                // Chiamata all'API per ottenere i tag del cliente
                var tags = await _httpClient.GetFromJsonAsync<List<string>>($"http://localhost:5202/api/Customer/{customerId}/tags");

                // Se non ci sono tag, restituiamo una lista vuota
                return tags ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer tags: {ex.Message}");
                return new List<string>(); // Restituisce una lista vuota in caso di errore
            }
        }


        // Ottieni i progetti di un cliente
        public async Task<List<Project>> GetCustomerProjects(int customerId)
        {
            try
            {
                // Chiamata all'API per ottenere i progetti del cliente
                var projects = await _httpClient.GetFromJsonAsync<List<Project>>($"http://localhost:5202/api/Customer/{customerId}/projects");

                // Se non ci sono progetti, restituiamo una lista vuota
                return projects ?? new List<Project>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching customer projects: {ex.Message}");
                return new List<Project>(); // Restituisce una lista vuota in caso di errore
            }
        }
    }
}
