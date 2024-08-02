﻿using System;

namespace Project3.Models
{
    public partial class VehicleInformation
    {
        public int Id { get; set; }
        public string? VehicleName { get; set; }
        public string? VehicleOwnerName { get; set; }
        public string? VehicleModel { get; set; }
        public string? VehicleVersion { get; set; }
        public decimal? VehicleRate { get; set; }
        public string? VehicleBodyNumber { get; set; }
        public string? VehicleEngineNumber { get; set; }
        public decimal? VehicleNumber { get; set; }
        public string? Location { get; set; } // New field
        public string? Usage { get; set; } // New field
        public string? WarrantyType { get; set; } // New field
        public string? PolicyType { get; set; } // New field
    }
}
