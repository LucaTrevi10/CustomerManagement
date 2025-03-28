using System;
using System.Collections.Generic;

namespace CustomerManagementApi.Data.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    // Navigazione verso CustomerTags
    public ICollection<CustomerTags> CustomerTags { get; set; } = new List<CustomerTags>();
}
