namespace CustomerUI.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
    }
}
