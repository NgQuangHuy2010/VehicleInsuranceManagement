using System.ComponentModel.DataAnnotations;

namespace Project3.ModelsView
{
    public class CompanyBillingPolicyViewModel
    {
        public int Id { get; set; }

        public string? CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? PolicyNumber { get; set; }

        public string? CustomerAddProve { get; set; }

        public string? CustomerPhoneNumber { get; set; }

        public string? BillNo { get; set; }

        public string? VehicleName { get; set; }

        public string? VehicleModel { get; set; }
        public string? VehicleVersion { get; set; }

        public float VehicleRate { get; set; }

        public string? VehicleBodyNumber { get; set; }

        public string? VehicleEngineNumber { get; set; }

        public DateTime? Date { get; set; }

        public float  Amount { get; set; }
        public string? PaymentStatus { get; set; } 


    }
}
