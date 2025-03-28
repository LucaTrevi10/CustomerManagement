namespace CustomerManagementApp.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int ProjectId { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateOnly PaymentDate { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string? Invoice { get; set; }

        public virtual Project Project { get; set; } = null!;

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
