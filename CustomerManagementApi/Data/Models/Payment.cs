namespace CustomerManagementApi.Data.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDueDate { get; set; }

        // Relazione con Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        // Relazione con Project
        public Project Project { get; set; }
    }
}
