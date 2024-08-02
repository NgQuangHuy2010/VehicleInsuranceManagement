using System;

namespace Project3.Models
{
    public partial class ClaimDetail
    {
        public int Id { get; set; }

        public decimal? ClaimNumber { get; set; }

        public decimal? PolicyNumber { get; set; }

        public DateTime? PolicyStartDate { get; set; } // Changed from DateOnly to DateTime

        public DateTime? PolicyEndDate { get; set; } // Changed from DateOnly to DateTime

        public string? CustomerName { get; set; }

        public string? PlaceOfAccident { get; set; }

        public DateTime? DateOfAccident { get; set; } // Changed from string to DateTime

        public decimal? InsuredAmount { get; set; }

        public decimal? ClaimableAmount { get; set; }
    }
}
