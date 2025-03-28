namespace CustomerManagementApi.Data.Models
{
    public class Project
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public DateTime? PaymentDate { get; set; }  // Nullabile per i progetti non ancora pagati
        public decimal Amount { get; set; }
        public string Status { get; set; }

        // Relazione con Customer
        public Customer Customer { get; set; }

        // Relazione con Payments
        public ICollection<Payment> Payments { get; set; }
    }
}
