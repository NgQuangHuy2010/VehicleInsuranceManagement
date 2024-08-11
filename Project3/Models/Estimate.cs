using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class Estimate
{
    public string CustomerId { get; set; } = null!;

    public int EstimateNumber { get; set; }

    public string? CustomeName { get; set; }

    public string? CustomerPhoneNumber { get; set; }

    public string? VehicleName { get; set; }

    public string? VehicleModel { get; set; }

    public double VehicleRate { get; set; }

    public int? WarrantyId { get; set; }

    public int PolicyTypeId { get; set; }

    public int VehicleId { get; set; }
}
