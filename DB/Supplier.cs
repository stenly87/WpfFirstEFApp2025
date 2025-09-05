using System;
using System.Collections.Generic;

namespace WpfFirstEFApp.DB;

public partial class Supplier
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
