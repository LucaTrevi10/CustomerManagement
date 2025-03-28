using CustomerManagementApi.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CustomerManagementApi.Data.Dtos;
using System.Net.Http;

namespace CustomerManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private static List<Customer> _customers = new List<Customer>();
        private static List<Tag> _tags = new List<Tag>();
        private static List<CustomerTags> _customerTags = new List<CustomerTags>();
        private static List<Project> _projects = new List<Project>();
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/customers
        [HttpGet]
        public ActionResult<IEnumerable<CustomerDto>> GetCustomers()
        {
            var customerDtos = _customers.Select(customer =>
            {
                var dto = new CustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Tags = _customerTags
                        .Where(ct => ct.CustomerId == customer.Id)
                        .Select(ct => _tags.First(t => t.Id == ct.TagId).Name)
                        .ToList()
                };
                return dto;
            });

            return Ok(customerDtos);
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        public ActionResult<CustomerDto> GetCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Tags = _customerTags
                    .Where(ct => ct.CustomerId == customer.Id)
                    .Select(ct => _tags.First(t => t.Id == ct.TagId).Name)
                    .ToList()
            };

            return Ok(customerDto);
        }

        // POST: api/customers
        [HttpPost]
        public ActionResult<CustomerDto> AddCustomer(CustomerDto customerDto)
        {
            if (_customers.Any(c => c.Email == customerDto.Email))
            {
                return Conflict("Un cliente con questa email esiste già.");
            }

            var newCustomer = new Customer
            {
                Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1,
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email
            };

            _customers.Add(newCustomer);
            return CreatedAtAction(nameof(GetCustomer), new { id = newCustomer.Id }, customerDto);
        }

        // PUT: api/customers/5
        [HttpPut("{id}")]
        public IActionResult PutCustomer(int id, CustomerDto customerDto)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;

            return NoContent();
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerTags.RemoveAll(ct => ct.CustomerId == id);
            _customers.Remove(customer);

            return NoContent();
        }

        // POST: api/customers/{customerId}/tags
        [HttpPost("{customerId}/tags/{tagName}")]
        public IActionResult AddTagToCustomer(int customerId, string tagName)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
            {
                return NotFound("Cliente non trovato.");
            }

            var tag = _tags.FirstOrDefault(t => t.Name == tagName) ?? new Tag
            {
                Id = _tags.Any() ? _tags.Max(t => t.Id) + 1 : 1,
                Name = tagName
            };

            if (!_tags.Contains(tag))
            {
                _tags.Add(tag);
            }

            if (_customerTags.Any(ct => ct.CustomerId == customerId && ct.TagId == tag.Id))
            {
                return Conflict("Il cliente ha già questo tag.");
            }

            _customerTags.Add(new CustomerTags
            {
                CustomerId = customerId,
                TagId = tag.Id
            });

            return Ok("Tag aggiunto al cliente.");
        }

        // DELETE: api/customers/{customerId}/tags/{tagName}
        [HttpDelete("{customerId}/tags/{tagName}")]
        public IActionResult RemoveTagFromCustomer(int customerId, string tagName)
        {
            var customer = _context.Customers.Include(c => c.CustomerTags).FirstOrDefault(c => c.Id == customerId);
            if (customer == null) return NotFound("Customer not found");

            var tag = _context.Tags.FirstOrDefault(t => t.Name == tagName);
            if (tag == null) return NotFound("Tag not found");

            var customerTag = customer.CustomerTags.FirstOrDefault(ct => ct.TagId == tag.Id);
            if (customerTag != null)
            {
                customer.CustomerTags.Remove(customerTag);
                _context.SaveChanges();
            }

            return Ok();
        }


        [HttpGet("{customerId}/projects")]
        public IActionResult GetCustomerProjects(int customerId)
        {
            var projects = _context.Projects.Where(p => p.CustomerId == customerId).ToList();
            return Ok(projects);
        }

        [HttpPost("{customerId}/tags")]
        public IActionResult AddTagToCustomer(int customerId, [FromBody] Tag tag)
        {
            var customer = _context.Customers.Include(c => c.CustomerTags).FirstOrDefault(c => c.Id == customerId);
            if (customer == null) return NotFound("Customer not found");

            // Trova il tag esistente o creane uno nuovo
            var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tag.Name);
            if (existingTag == null)
            {
                existingTag = new Tag { Name = tag.Name };
                _context.Tags.Add(existingTag);
                _context.SaveChanges();
            }

            // Associa il tag al cliente se non già associato
            if (!customer.CustomerTags.Any(ct => ct.TagId == existingTag.Id))
            {
                customer.CustomerTags.Add(new CustomerTags { TagId = existingTag.Id });
                _context.SaveChanges();
            }

            return Ok();
        }

        [HttpGet("{customerId}/tags")]
        public IActionResult GetCustomerTags(int customerId)
        {
            var customer = _context.Customers.Include(c => c.CustomerTags)
                                             .ThenInclude(ct => ct.Tag)
                                             .FirstOrDefault(c => c.Id == customerId);
            if (customer == null) return NotFound("Customer not found");

            var tags = customer.CustomerTags.Select(ct => ct.Tag.Name).ToList();
            return Ok(tags);
        }

        // Metodi helper
        private bool CustomerExists(int id)
        {
            return _customers.Any(c => c.Id == id);
        }
    }
}
