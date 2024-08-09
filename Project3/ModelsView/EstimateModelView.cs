using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class EstimateModelView
    {
        public string CustomerId { get; set; }
        public int EstimateNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string VehicleName { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleVersion { get; set; }
        public float VehicleRate { get; set; }
        [Required]
        public int? WarrantyId { get; set; }
        [Required]
        public int PolicyTypeId { get; set; }
        public int VehicleId { get; set; }
        // Add this property to hold the policies data

        public bool Policies { get; set; }
    }
}
