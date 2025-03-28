using System;
using System.Collections.Generic;

namespace CustomerManagementApi.Data.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    //public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

    // Navigazione verso i Tag
    public ICollection<CustomerTags> CustomerTags { get; set; } = new List<CustomerTags>();

    // Navigazione verso i progetti
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
