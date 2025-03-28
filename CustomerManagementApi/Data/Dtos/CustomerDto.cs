using CustomerManagementApi.Data.Models;

namespace CustomerManagementApi.Data.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? DateOfBirth { get; set; } // Convertito in stringa per la serializzazione
        public string? Address { get; set; }

        // Lista di tag associati al cliente     
        public List<string> Tags { get; set; } = new();

        // Aggiungi la proprietà per i progetti
        public List<ProjectDto> Projects { get; set; } = new();
    }
}
