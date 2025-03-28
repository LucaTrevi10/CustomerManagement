using CustomerService.Data;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerServiceDbContext _context;

        public CustomerController(CustomerServiceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomers()
        {
            try
            {
                return await _context.Customer.ToListAsync();
            }
            catch(Exception ex)
            { return BadRequest(); }
        }
      
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerDto(int id)
        {
            try
            {
                var customer = await _context.Customer
               .Where(c => c.CustomerId == id)
               .Select(c => new CustomerDto
               {
                   CustomerId = c.CustomerId,
                   CompanyName = c.CompanyName
               })
               .FirstOrDefaultAsync();

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch(Exception ex)
            { return BadRequest(); }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            try
            {
                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomerDto", new { id = customer.CustomerId }, customer);
            }
            catch (Exception ex)
            { return BadRequest("Error i n ethod PostCustomer."); }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            try
            {
                if (id != customer.CustomerId)
                {
                    return BadRequest();
                }

                _context.Entry(customer).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            { return BadRequest("Error in method PutCustomer."); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var customer = await _context.Customer.FindAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                _context.Customer.Remove(customer);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            { return BadRequest("Error in method DeleteCustomer."); }
        }

        private bool CustomerExists(int id)
        {
            try
            {
                return _context.Customer.Any(e => e.CustomerId == id);
            }
            catch(Exception ex)
            {
                return true;
            }
        }
    }
}
