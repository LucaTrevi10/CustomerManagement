using CustomerManagementApp.Models;

namespace CustomerManagementApp.Services.Mock
{
    public class ProjectService : IProjectService
    {
        private List<Project> _projects = new List<Project>
    {
        new Project { ProjectId = 1, CustomerId = 1, ProjectName = "Website Redesign", StartDate = new DateOnly(2025, 1, 13), EndDate = new DateOnly(2024, 06, 15), PaymentDueDate = new DateOnly(2024, 07, 01)},
        new Project { ProjectId = 2, CustomerId = 1, ProjectName = "Mobile App Development", StartDate = new DateOnly(2024, 02, 01), EndDate = new DateOnly(2024, 09, 01), PaymentDueDate = new DateOnly(2024, 10, 01) },
        new Project { ProjectId = 3, CustomerId = 2, ProjectName = "SEO Optimization", StartDate = new DateOnly(2024, 03, 01), EndDate = new DateOnly(2024, 05, 01), PaymentDueDate = new DateOnly(2024, 06, 01)}
    };

        public Task<List<Project>> GetCustomerProjects(int customerId)
        {
            return Task.FromResult(_projects.Where(p => p.CustomerId == customerId).ToList());
        }

        public void AddProject(Project project)
        {
            _projects.Add(project);  // Aggiungi il progetto alla lista
        }
    }
}
