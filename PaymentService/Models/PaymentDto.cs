namespace PaymentService.Models
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public string ProjectName { get; set; } = null!;
        public decimal PaymentAmount { get; set; }
        public DateOnly PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string? Invoice { get; set; }
        public string? InvoiceFilePath { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; } = null!;
    }
}
