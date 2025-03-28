namespace CustomerUI.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        public int CustomerId { get; set; }

        public string ProjectName { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public DateOnly PaymentDueDate { get; set; }

        public bool IsComplete { get; set; }

        public int? PaymentId { get; set; }

        public virtual Customer? Customer { get; set; } = null!;

        public virtual Payment? Payment { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
