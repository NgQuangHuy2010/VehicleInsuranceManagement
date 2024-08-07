using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

public class EstimateViewModel
{
    public string CustomerId { get; set; }
    public int EstimateNumber { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhoneNumber { get; set; }
    public string VehicleName { get; set; }
    public string VehicleModel { get; set; }
    public decimal? VehicleRate { get; set; }
    public int? WarrantyId { get; set; }
    public int PolicyTypeId { get; set; }
    public int VehicleId { get; set; }
    public IEnumerable<SelectListItem> Warranties { get; set; }
    public IEnumerable<SelectListItem> PolicyTypes { get; set; }
}
