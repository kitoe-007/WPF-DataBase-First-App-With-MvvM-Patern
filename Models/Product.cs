using System;
using System.Collections.Generic;

namespace DataBaseFirstWpfApp.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? Manufacturer { get; set; }

    public string? StorageConditions { get; set; }

    public string? Packaging { get; set; }

    public int? ShelfLife { get; set; }

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
