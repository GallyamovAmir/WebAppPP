using System;
using System.Collections.Generic;

namespace WebAppPP.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ProductCategory Category { get; set; } = null!;
}
