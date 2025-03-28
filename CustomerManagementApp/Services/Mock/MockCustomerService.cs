using CustomerManagementApp.Models;

namespace CustomerManagementApp.Services.Mock
{
    public class MockCustomerService //: ICustomerService
    {
        private readonly List<Customer> _mockCustomers;

        public MockCustomerService()
        {
            // Dati di mock
            //_mockCustomers = new List<CustomerDto>
            //{
            //    new CustomerDto
            //    {
            //        Id = 1,
            //        FirstName = "Acme Corp.",
            //        LastName = "Inc.",
            //        Email = "contact@acmecorp.com",
            //        Phone = "1234567890",
            //        DateOfBirth = "2000-01-15", // Data di fondazione dell'azienda
            //        Address = "123 Main St, Springfield",
            //        Tags = new List<string> { "VIP", "Long-term Partner" },
            //        Projects = new List<ProjectDto>
            //        {
            //            new ProjectDto
            //            {
            //                Id = 1,
            //                Name = "Project Alpha",
            //                StartDate = DateTime.Now.AddDays(-30),
            //                EndDate = DateTime.Now.AddDays(30),
            //                PaymentDueDate = DateTime.Now.AddDays(15),
            //                PaymentDate = null
            //            }
            //        }
            //    }
            //};            
        }

        //public Task<List<CustomerDto>> GetCustomers()
        //{
        //    return Task.FromResult(_mockCustomers);
        //}

        //public Task<CustomerDto> GetCustomer(int id)
        //{
        //    var customer = _mockCustomers.FirstOrDefault(c => c.Id == id);
        //    return Task.FromResult(customer ?? throw new Exception("Customer not found"));
        //}

        //public Task AddCustomer(CustomerDto customerDto)
        //{
        //    customerDto.Id = _mockCustomers.Max(c => c.Id) + 1;
        //    _mockCustomers.Add(customerDto);
        //    return Task.CompletedTask;
        //}

        //public Task UpdateCustomer(CustomerDto customerDto)
        //{
        //    var existingCustomer = _mockCustomers.FirstOrDefault(c => c.Id == customerDto.Id);
        //    if (existingCustomer == null)
        //    {
        //        throw new Exception("Customer not found");
        //    }

        //    _mockCustomers.Remove(existingCustomer);
        //    _mockCustomers.Add(customerDto);
        //    return Task.CompletedTask;
        //}

        public Task DeleteCustomer(int id)
        {
            //var customer = _mockCustomers.FirstOrDefault(c => c.Id == id);
            //if (customer == null)
            //{
            //    throw new Exception("Customer not found");
            //}

            //_mockCustomers.Remove(customer);
            return Task.CompletedTask;
        }

        public Task AddTagToCustomer(int customerId, string tagName)
        {
            //var customer = _mockCustomers.FirstOrDefault(c => c.Id == customerId);
            //if (customer == null)
            //{
            //    throw new Exception("Customer not found");
            //}

            //if (!customer.Tags.Contains(tagName))
            //{
            //    customer.Tags.Add(tagName);
            //}

            return Task.CompletedTask;
        }

        public Task RemoveTagFromCustomer(int customerId, string name)
        {
            //var customer = _mockCustomers.FirstOrDefault(c => c.Id == customerId);
            //if (customer == null)
            //{
            //    throw new Exception("Customer not found");
            //}

            //customer.Tags.Remove(name);
            return Task.CompletedTask;
        }

        //public Task<List<string>> GetCustomerTags(int customerId)
        //{
        //    var customer = _mockCustomers.FirstOrDefault(c => c.CustomerId == customerId);
        //    //if (customer == null)
        //    //{
        //    //    throw new Exception("Customer not found");
        //    //}

        //    return Task.FromResult(customer.Tags);
        //}

        //public Task<Dictionary<string, int>> GetCustomerCountsByMonth()
        //{
        //    var countsByMonth = 0;

        //    return Task.FromResult(countsByMonth);
        //}

        //public Task<List<ProjectDto>> GetCustomerProjects(int customerId)
        //{
        //    var projects = new List<Project>
        //    {
        //         new Project
        //         {
        //             Id = 1,
        //             CustomerId = 1,
        //             ProjectName = "Website Redesign",
        //             StartDate = new DateTime(2024, 01, 10),
        //             EndDate = new DateTime(2024, 06, 15),
        //             PaymentDueDate = new DateTime(2024, 07, 01),
        //             PaymentDate = null,
        //             Amount = 5000m,
        //             Status = "In Progress"
        //         }
        //    };

        //    var projectDtos = projects
        //        .Where(p => p.CustomerId == customerId)
        //        .Select(p => new ProjectDto
        //        {
        //            Id = p.Id,
        //            Name = p.ProjectName,
        //            StartDate = p.StartDate,
        //            EndDate = p.EndDate,
        //            PaymentDueDate = p.PaymentDueDate,
        //            PaymentDate = p.PaymentDate
        //        })
        //        .ToList();

        //    return Task.FromResult(projectDtos);
        //}
    }


}
