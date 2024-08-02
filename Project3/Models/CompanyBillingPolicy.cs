using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class CompanyBillingPolicy
{
    public int Id { get; set; }

    public string? CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public decimal? PolicyNumber { get; set; }

    public string? CustomerAddProve { get; set; }

    public decimal? CustomerPhoneNumber { get; set; }

    public decimal? BillNo { get; set; }

    public string? VehicleName { get; set; }

    public string? VehicleModel { get; set; }

    public decimal? VehicleRate { get; set; }

    public string? VehicleBodyNumber { get; set; }

    public string? VehicleEngineNumber { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Amount { get; set; }

    public virtual AspNetUser? Customer { get; set; }
}
