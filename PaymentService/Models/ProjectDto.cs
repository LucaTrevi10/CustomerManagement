namespace PaymentService.Models
{
    public class ProjectDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly PaymentDueDate { get; set; }
        public string CompanyName { get; set; } // Nome del cliente
        public int CustomerId { get; set; }
        public bool IsComplete { get; set; }
    }
}
