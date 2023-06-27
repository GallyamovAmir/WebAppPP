using System;
using System.Collections.Generic;

namespace WebAppPP.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual User User { get; set; } = null!;
}
