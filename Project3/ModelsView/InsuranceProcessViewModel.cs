using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class InsuranceProcessViewModel
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [Phone]
        public string CustomerPhoneNumber { get; set; }
        [Required]
        public int VehicleId { get; set; }
        public string? PolicyNumber { get; set; }

        [Required]
        public string VehicleName { get; set; }

        [Required]
        public string VehicleModel { get; set; }

        [Required]
        public float VehicleRate { get; set; }
        [Required]
        public int PolicyTypeId { get; set; }
        [Required]
        public int? WarrantyId { get; set; }

        [Required]
        public string VehicleBodyNumber { get; set; }

        [Required]
        public string VehicleEngineNumber { get; set; }
        [Required]
        public string? PolicyDate { get; set; }
        
        public decimal? PolicyDuration { get; set; }

        
    }
}
