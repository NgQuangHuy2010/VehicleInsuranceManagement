using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class Estimate
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public decimal? EstimateNumber { get; set; }

    public string? CustomerName { get; set; }

    public decimal? CustomerPhoneNumber { get; set; }

    public string? VehicleName { get; set; }

    public string? VehicleModel { get; set; }

    public decimal? VehicleRate { get; set; }

    public string? VehicleWarranty { get; set; }

    public string? VehiclePolicyType { get; set; }
}
