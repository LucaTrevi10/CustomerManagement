using CustomerManagementApp.Models;
namespace CustomerManagementApp.Services.Mock
{
    public interface IProjectService
    {
        Task<List<Project>> GetCustomerProjects(int customerId);
        void AddProject(Project project);  // Metodo per aggiungere un progetto
    }
}
