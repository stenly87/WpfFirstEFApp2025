using System;
using System.Collections.Generic;

namespace WpfFirstEFApp.DB;

public partial class Product
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public decimal? Price { get; set; }

    public byte[]? Image { get; set; }

    public int? CategoryId { get; set; }

    public int? SupplierId { get; set; }

    public int? Count { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
