namespace CustomerManagementApi.Data.Dtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
