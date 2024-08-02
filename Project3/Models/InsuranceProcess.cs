using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class InsuranceProcess
{
    public int Id { get; set; }

    public string? CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerAdd { get; set; }

    public decimal? CustomerPhoneNumber { get; set; }

    public decimal? PolicyNumber { get; set; }

    public string? PolicyDate { get; set; }

    public decimal? PolicyDuration { get; set; }

    public decimal? VehicleNumber { get; set; }

    public string? VehicleName { get; set; }

    public string? VehicleModel { get; set; }

    public string? VehicleVersion { get; set; }

    public decimal? VehicleRate { get; set; }

    public string? VehicleWarranty { get; set; }

    public string? VehicleBodyNumber { get; set; }

    public string? VehicleEngineNumber { get; set; }

    public string? CustomerAddProve { get; set; }

    public virtual AspNetUser? Customer { get; set; }
}
