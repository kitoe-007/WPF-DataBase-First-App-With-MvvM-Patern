using System;
using System.Collections.Generic;

namespace DataBaseFirstWpfApp.Models;

public partial class Warehouse
{
    public int Id { get; set; }

    public DateTime ReceiptDate { get; set; }

    public int ProductId { get; set; }

    public int SupplierId { get; set; }

    public string? DeliveryMethod { get; set; }

    public int Volume { get; set; }

    public double? Price { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
