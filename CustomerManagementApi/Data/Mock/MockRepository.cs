using CustomerManagementApi.Data.Models;
using System.Xml.Linq;

namespace CustomerManagementApi.Data.Mock
{
    // Mock repository per gestire i dati
    public class MockRepository
    {
        private readonly List<Customer> _customers;
        private readonly List<Tag> _tags;

        public MockRepository()
        {
            // Dati mock per i Customer
            _customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                FirstName = "Mario",
                LastName = "Rossi",
                Email = "mario.rossi@example.com",
                Phone = "1234567890",
                DateOfBirth = new DateOnly(1990, 5, 20),
                Address = "Via Roma, 10",
                Projects = new List<Project>
                {
                    new Project
                    {
                        Id = 1,
                        CustomerId = 1,
                        ProjectName = "Progetto A",
                        StartDate = DateTime.Now.AddMonths(-2),
                        EndDate = DateTime.Now.AddMonths(1),
                        PaymentDueDate = DateTime.Now.AddDays(30),
                        PaymentDate = null,
                        Amount = 1500.00m,
                        Status = "In corso",
                        Payments = new List<Payment>
                        {
                            new Payment
                            {
                                Id = 1,
                                ProjectId = 1,
                                CustomerId = 1,
                                PaymentAmount = 500.00m,
                                PaymentDate = DateTime.Now.AddDays(-10),
                                PaymentStatus = "Pagato",
                                PaymentMethod = "Carta di credito",
                                PaymentDueDate = DateTime.Now.AddDays(30)
                            }
                        }
                    }
                },
                CustomerTags = new List<CustomerTags>
                {
                    new CustomerTags { CustomerId = 1, TagId = 1 }
                }
            },
            new Customer
            {
                Id = 2,
                FirstName = "Giulia",
                LastName = "Verdi",
                Email = "giulia.verdi@example.com",
                Phone = "0987654321",
                DateOfBirth = new DateOnly(1985, 7, 15),
                Address = "Piazza Dante, 5",
                Projects = new List<Project>(),
                CustomerTags = new List<CustomerTags>()
            }
        };

            // Dati mock per i Tag
            _tags = new List<Tag>
        {
            new Tag { Id = 1, Name = "VIP" },
            new Tag { Id = 2, Name = "Nuovo Cliente" }
        };
        }

        // Metodi CRUD per Customer
        public List<Customer> GetCustomers() => _customers;

        public Customer? GetCustomerById(int id) => _customers.FirstOrDefault(c => c.Id == id);

        public void AddCustomer(Customer customer)
        {
            customer.Id = _customers.Max(c => c.Id) + 1;
            _customers.Add(customer);
        }

        public bool UpdateCustomer(Customer updatedCustomer)
        {
            var customer = GetCustomerById(updatedCustomer.Id);
            if (customer == null) return false;

            customer.FirstName = updatedCustomer.FirstName;
            customer.LastName = updatedCustomer.LastName;
            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;
            customer.DateOfBirth = updatedCustomer.DateOfBirth;
            customer.Address = updatedCustomer.Address;

            return true;
        }

        public bool DeleteCustomer(int id)
        {
            var customer = GetCustomerById(id);
            if (customer == null) return false;

            _customers.Remove(customer);
            return true;
        }

        // Metodi per i Tag
        public List<Tag> GetTags() => _tags;

        public Tag? GetTagById(int id) => _tags.FirstOrDefault(t => t.Id == id);

        // Aggiunge un tag a un cliente
        public bool AddTagToCustomer(int customerId, int tagId)
        {
            var customer = GetCustomerById(customerId);
            var tag = GetTagById(tagId);
            if (customer == null || tag == null) return false;

            customer.CustomerTags.Add(new CustomerTags { CustomerId = customerId, TagId = tagId });
            return true;
        }

        // Rimuove un tag da un cliente
        public bool RemoveTagFromCustomer(int customerId, int tagId)
        {
            var customer = GetCustomerById(customerId);
            if (customer == null) return false;

            var customerTag = customer.CustomerTags.FirstOrDefault(ct => ct.TagId == tagId);
            if (customerTag == null) return false;

            customer.CustomerTags.Remove(customerTag);
            return true;
        }

        // Metodi per i Progetti
        public bool AddProjectToCustomer(int customerId, Project project)
        {
            var customer = GetCustomerById(customerId);
            if (customer == null) return false;

            project.Id = customer.Projects.Any() ? customer.Projects.Max(p => p.Id) + 1 : 1;
            customer.Projects.Add(project);
            return true;
        }

        public bool RemoveProjectFromCustomer(int customerId, int projectId)
        {
            var customer = GetCustomerById(customerId);
            if (customer == null) return false;

            var project = customer.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null) return false;

            customer.Projects.Remove(project);
            return true;
        }

        // Segna un pagamento come effettuato
        public bool MarkPaymentAsComplete(int customerId, int projectId, Payment payment)
        {
            var customer = GetCustomerById(customerId);
            if (customer == null) return false;

            var project = customer.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project == null) return false;

            payment.Id = project.Payments.Any() ? project.Payments.Max(p => p.Id) + 1 : 1;
            project.Payments.Add(payment);

            return true;
        }
    }

}
