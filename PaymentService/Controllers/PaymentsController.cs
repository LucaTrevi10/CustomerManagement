using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : Controller
    {
        private readonly PaymentDbContext _context;
        private readonly HttpClient _httpClient;

        public PaymentsController(PaymentDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<List<PaymentDto>>> GetPayments()
        {
            try
            {
                var payments = await _context.Payments.ToListAsync();

                // Creiamo una lista di PaymentDto con i dati base
                var paymentDtos = new List<PaymentDto>();

                foreach (var payment in payments)
                {
                    // Chiamiamo il microservizio ProjectService per ottenere i dettagli del progetto
                    var projectResponse = await _httpClient.GetFromJsonAsync<ProjectDto>(
                        $"http://localhost:5177/api/project/{payment.ProjectId}");

                    // Se il progetto non è trovato, saltiamo l'iterazione
                    if (projectResponse == null)
                    {
                        continue;
                    }

                    // Chiamiamo il microservizio CustomerService per ottenere i dettagli del cliente
                    var customerResponse = await _httpClient.GetFromJsonAsync<CustomerDto>(
                        $"http://localhost:5089/api/customer/{projectResponse.CustomerId}");

                    // Creiamo l'oggetto PaymentDto con i dati ottenuti
                    var paymentDto = new PaymentDto
                    {
                        PaymentId = payment.PaymentId,
                        ProjectName = projectResponse.ProjectName,
                        PaymentAmount = payment.PaymentAmount,
                        PaymentDate = payment.PaymentDate,
                        PaymentMethod = payment.PaymentMethod,
                        Invoice = payment.Invoice,
                        InvoiceFilePath = payment.InvoiceFilePath,
                        Status = payment.Status,
                        CustomerName = customerResponse?.CompanyName ?? "Sconosciuto"
                    };

                    paymentDtos.Add(paymentDto);
                }

                return Ok(paymentDtos);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }  
        }

        // GET: api/payments/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null)
                    return NotFound();

                var projectResponse = await _httpClient.GetFromJsonAsync<ProjectDto>($"http://localhost:5177/api/project/{payment.ProjectId}");
                if (projectResponse == null)
                    return NotFound("Project not found");

                var customerResponse = await _httpClient.GetFromJsonAsync<CustomerDto>($"http://localhost:5089/api/customer/{projectResponse.CustomerId}");

                var paymentDto = new PaymentDto
                {
                    PaymentId = payment.PaymentId,
                    ProjectName = projectResponse.ProjectName,
                    PaymentAmount = payment.PaymentAmount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod,
                    Invoice = payment.Invoice,
                    InvoiceFilePath = payment.InvoiceFilePath,
                    Status = payment.Status,
                    CustomerName = customerResponse?.CompanyName ?? "Sconosciuto"
                };

                return Ok(paymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}/{Amount}/{Date}/{Method}/{Status}")]
        public async Task<IActionResult> UpdatePayment(int id, decimal? Amount, string Date, string Method, string Status)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
                return NotFound();

            if (Amount != null)
                payment.PaymentAmount = (decimal)Amount;

            if (!string.IsNullOrWhiteSpace(Date) && DateOnly.TryParse(Date, out var parsedDate))
                payment.PaymentDate = parsedDate;

            if (!String.IsNullOrEmpty(Method))
                payment.PaymentMethod = Method;

            if (!String.IsNullOrEmpty(Status))
                payment.Status = Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Payments.Any(p => p.PaymentId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            _context.Payments.Remove(payment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error deleting payment.");
            }

            return NoContent();
        }

        // PUT: api/payments/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] string status)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return NotFound();

            payment.Status = status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
