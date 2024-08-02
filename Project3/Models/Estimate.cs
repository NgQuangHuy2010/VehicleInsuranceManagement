using System;
using System.Collections.Generic;

namespace Project3.Models
{
public partial class Estimate
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public decimal? EstimateNumber { get; set; }

    public string? CustomerName { get; set; }

    public decimal? CustomerPhoneNumber { get; set; }

    public string? VehicleName { get; set; }

    public string? VehicleModel { get; set; }

        public string? VehicleVersion { get; set; }

    public decimal? VehicleRate { get; set; }

        public string? VehicleBodyNumber { get; set; }

        public string? VehicleEngineNumber { get; set; }

        public decimal? VehicleNumber { get; set; }

    public string? VehicleWarranty { get; set; }

    public string? VehiclePolicyType { get; set; }

        // Additional Properties
        public int DriverAge { get; set; }

        public string DriverGender { get; set; }

        public string Location { get; set; }

        public string Usage { get; set; }

        public bool AntiTheftDevice { get; set; }

        public List<string> SelectedCoverages { get; set; }

        public int DrivingHistory { get; set; }

        public bool? MultiPolicy { get; set; }
        public bool? SafeDriver { get; set; }

        public decimal EstimatedCost { get; set; }
    }
}
