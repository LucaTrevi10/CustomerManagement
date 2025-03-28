namespace CustomerManagementApp.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string CompanyName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
