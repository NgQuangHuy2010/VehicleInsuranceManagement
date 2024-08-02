//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Project3.Models;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Project3.Controllers
//{
//    [Route("[controller]")]
//    public class EstimateController : Controller
//    {
//        private readonly VehicleInsuranceManagementContext _context;

//        public EstimateController(VehicleInsuranceManagementContext context)
//        {
//            _context = context;
//        }

//        [Route("Index")]
//        public async Task<IActionResult> Index()
//        {
//            var estimates = await _context.Estimates.ToListAsync();
//            return View(estimates);
//        }
//        [HttpGet]
//        public IActionResult Create(string VehicleName, string VehicleModel, string VehicleVersion, decimal? VehicleRate, string VehicleBodyNumber, string VehicleEngineNumber, decimal? VehicleNumber, string Usage, string Location, string WarrantyType, string PolicyType)
//        {
//            var estimate = new Estimate
//            {
//                VehicleName = VehicleName,
//                VehicleModel = VehicleModel,
//                VehicleVersion = VehicleVersion,
//                VehicleRate = VehicleRate,
//                VehicleBodyNumber = VehicleBodyNumber,
//                VehicleEngineNumber = VehicleEngineNumber,
//                VehicleNumber = VehicleNumber,
//                Usage = Usage,
//                Location = Location,
//                VehicleWarranty = WarrantyType,
//                VehiclePolicyType = PolicyType,
//                SelectedCoverages = new List<string>() // Ensure this is initialized
//            };

//            estimate.EstimatedCost = CalculateEstimateCost(estimate);

//            return View(estimate);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("VehicleName,VehicleModel,VehicleVersion,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleNumber,DriverAge,DriverGender,Location,Usage,AntiTheftDevice,SelectedCoverages,DrivingHistory,MultiPolicy,SafeDriver,VehicleWarranty,VehiclePolicyType,EstimatedCost")] Estimate estimate)
//        {
//            if (ModelState.IsValid)
//            {
//                var baseRate = CalculateBaseRate(estimate);
//                var totalCost = ApplyRiskFactors(baseRate, estimate);
//                var finalEstimate = ApplyDiscountsAndSurcharges(totalCost, estimate);

//                estimate.EstimatedCost = finalEstimate;

//                _context.Estimates.Add(estimate);
//                await _context.SaveChangesAsync();

//                return RedirectToAction(nameof(Index));
//            }
//            return View(estimate);
//        }

//        private decimal CalculateBaseRate(Estimate estimate)
//        {
//            decimal baseRate = estimate.VehicleRate ?? 0m;

//            if (estimate.VehicleRate.HasValue)
//            {
//                baseRate *= estimate.VehicleRate.Value / 1000;
//            }

//            if (estimate.DriverAge < 25 || estimate.DriverAge > 65)
//            {
//                baseRate *= 1.2m;
//            }
//            if (estimate.DriverGender == "Male")
//            {
//                baseRate *= 1.1m;
//            }
//            if (estimate.DrivingHistory > 0)
//            {
//                baseRate *= 1 + (estimate.DrivingHistory * 0.05m);
//            }

//            return baseRate;
//        }

//        private decimal ApplyRiskFactors(decimal baseRate, Estimate estimate)
//        {
//            if (estimate.Location == "Urban")
//            {
//                baseRate *= 1.2m;
//            }

//            if (estimate.Usage == "Daily")
//            {
//                baseRate *= 1.15m;
//            }

//            if (estimate.AntiTheftDevice)
//            {
//                baseRate *= 0.95m;
//            }

//            return baseRate;
//        }

//        private decimal ApplyDiscountsAndSurcharges(decimal totalCost, Estimate estimate)
//        {
//            if (estimate.SelectedCoverages != null)
//            {
//                foreach (var coverage in estimate.SelectedCoverages)
//                {
//                    switch (coverage)
//                    {
//                        case "Liability":
//                            totalCost *= 1.1m; // 10% increase for liability coverage
//                            break;
//                        case "Collision":
//                            totalCost *= 1.2m; // 20% increase for collision coverage
//                            break;
//                        case "Comprehensive":
//                            totalCost *= 1.15m; // 15% increase for comprehensive coverage
//                            break;
//                    }
//                }
//            }

//            if (estimate.MultiPolicy.HasValue && estimate.MultiPolicy.Value)
//            {
//                totalCost *= 0.9m; // 10% discount for multi-policy
//            }

//            if (estimate.SafeDriver.HasValue && estimate.SafeDriver.Value)
//            {
//                totalCost *= 0.85m; // 15% discount for safe driver
//            }

//            return totalCost;
//        }

//        private decimal CalculateEstimateCost(Estimate estimate)
//        {
//            var baseRate = CalculateBaseRate(estimate);
//            var totalCost = ApplyRiskFactors(baseRate, estimate);
//            return ApplyDiscountsAndSurcharges(totalCost, estimate);
//        }


//    }
//}
