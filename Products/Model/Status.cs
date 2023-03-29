using System;
using System.Collections.Generic;

namespace Products.Model;

public partial class Status
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
