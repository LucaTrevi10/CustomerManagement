using Blazored.LocalStorage;
using CustomerUI.Models;
using CustomerUI.Models.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CustomerUI.Services
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public PaymentService(IHttpClientFactory clientFactory, ILocalStorageService localStorage)
        {
            _httpClient = clientFactory.CreateClient("PaymentService");
            _localStorage = localStorage;
        }

        // Ottieni tutti i pagamenti
        public async Task<List<PaymentDto>> GetPaymentsAsync()
        {
            var response = await _httpClient.GetAsync("api/payments");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PaymentDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PaymentDto>();
        }

        // Ottieni un pagamento per ID
        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Payment>($"api/payments/{id}");
        }

        // Ottieni i pagamenti associati a un ProjectId
        public async Task<List<PaymentDto>> GetPaymentsByProjectIdAsync(int projectId)
        {
            var response = await _httpClient.GetAsync($"api/projects/{projectId}/payments");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PaymentDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<PaymentDto>();
        }

        // Crea un nuovo pagamento
        public async Task CreatePaymentAsync(Payment payment)
        {
            await _httpClient.PostAsJsonAsync("api/payments", payment);
        }

        // Aggiorna un pagamento
        public async Task UpdatePaymentAsync(PaymentDto payment)
        {
            // Converti PaymentDate in stringa con formato ISO 8601
            string paymentDate = payment.PaymentDate.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            // Costruisci l'URL
            string url = $"api/payments/{payment.PaymentId}/{payment.PaymentAmount}/{paymentDate}/{payment.PaymentMethod}/{payment.Status}";

            // Effettua la chiamata
            var response = await _httpClient.PutAsync(url, null); // Nessun corpo JSON necessario per l'endpoint PUT con parametri nell'URL

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to update payment.");
            }
        }


        // Elimina un pagamento
        public async Task DeletePaymentAsync(int paymentId)
        {
            var response = await _httpClient.DeleteAsync($"api/payments/{paymentId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to delete payment.");
            }
        }

        public async Task<HttpResponseMessage> UpdatePaymentStatus(int paymentId, string status)
        {
            var content = new StringContent(JsonSerializer.Serialize(status), Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync($"api/payments/{paymentId}/status", content);
        }



        // Metodo per l'upload della fattura (PDF)
        public async Task UploadInvoice(int paymentId, IBrowserFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using var content = new MultipartFormDataContent();

            // Aggiungi il file al contenuto
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)); // Max 10 MB
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "invoiceFile", file.Name);

            // Effettua la richiesta
            var response = await _httpClient.PostAsync($"api/payments/{paymentId}/upload-invoice", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error uploading invoice: {response.StatusCode}, {errorContent}");
            }
        }

        // Metodo per il download del file PDF
        public async Task<byte[]> DownloadInvoice(int paymentId)
        {
            var response = await _httpClient.GetAsync($"api/payments/{paymentId}/download-invoice");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                throw new Exception("Download failed");
            }
        }

        public async Task UploadExcelFileToServer(IBrowserFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using var content = new MultipartFormDataContent();

            // Aggiungi il file al contenuto
            var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024)); // Max 10 MB
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "file", file.Name);

            // Effettua la richiesta POST all'API
            var response = await _httpClient.PostAsync("api/payments/upload-excel", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error uploading payments: {response.StatusCode}, {errorContent}");
            }
        }

    }
}
