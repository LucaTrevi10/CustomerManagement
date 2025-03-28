using System.ComponentModel.DataAnnotations;

namespace ProjectService.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        public int CustomerId { get; set; }  // Relazione con il microservizio CustomerService

        [Required]
        public string ProjectName { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public DateOnly PaymentDueDate { get; set; }

        public bool IsComplete { get; set; }

        public int? PaymentId { get; set; }  // Relazione con il microservizio PaymentService

        // Le proprietà di navigazione vengono rimosse per evitare dipendenze dirette con altri microservizi
    }
}
