using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class VehicleWarranty
{
    public int WarrantyId { get; set; }

    public string? WarrantyType { get; set; }

    public string? WarrantyDuration { get; set; }

    public string? WarrantyDetails { get; set; }
}
