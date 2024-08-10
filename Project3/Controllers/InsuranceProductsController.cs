//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Project3;
//using Project3.Models;
//using Project3.ModelsView;

////[Authorize]
//[Route("[controller]")]
//public class InsuranceProductsController : Controller
//{
//    private readonly VehicleInsuranceManagementContext _context;
//    private readonly ILogger<InsuranceProductsController> _logger;

//    public InsuranceProductsController(VehicleInsuranceManagementContext context, ILogger<InsuranceProductsController> logger)
//    {
//        _context = context;
//        _logger = logger;
//    }

//    // GET: InsuranceProducts
//    [Route("index")]
//    [HttpGet]
//    public async Task<IActionResult> Index()
//    {
//        // Fetch all policy types and warranties
//        var policies = await _context.VehiclePolicyTypes.ToListAsync();
//        var warranties = await _context.VehicleWarranties.ToListAsync();

//        // Create a list of InsuranceProductViewModel to represent the product cards
//        var insuranceProducts = new List<InsuranceProductViewModel>();

        // Create combinations: pair each policy type with each warranty
        foreach (var policy in policies)
        {
            foreach (var warranty in warranties)
            {
                // Adjust the rate based on the warranty type or duration
                float adjustedRate = (float)policy.VehicleRate;

//                if (warranty.WarrantyDuration.Contains("3"))
//                {
//                    adjustedRate = adjustedRate;  // Add $50 for 5-year warranties
//                }
//                if (warranty.WarrantyDuration.Contains("5"))
//                {
//                    adjustedRate += 50;  // Add $50 for 5-year warranties
//                }
//                else if (warranty.WarrantyDuration.Contains("7"))
//                {
//                    adjustedRate += 100; // Add $100 for 7-year warranties
//                }

                insuranceProducts.Add(new InsuranceProductViewModel
                {
                    PolicyTypeId = policy.PolicyTypeId,
                    PolicyName = policy.PolicyName,
                    PolicyDetails = policy.PolicyDetails,
                    WarrantyId = warranty.WarrantyId,
                    WarrantyType = warranty.WarrantyType,
                    WarrantyDuration = warranty.WarrantyDuration,
                    WarrantyDetails = warranty.WarrantyDetails,
                    VehicleRate = adjustedRate // Use the adjusted rate
                });
            }
        }

//        return View(insuranceProducts);
//    }

//    [Route("confirmproduct")]
//    [HttpGet]
//    public async Task<IActionResult> Buy(int policyTypeId, int warrantyId)
//    {
//        // Fetch the selected policy and warranty from the database
//        var policy = await _context.VehiclePolicyTypes.FirstOrDefaultAsync(p => p.PolicyTypeId == policyTypeId);
//        var warranty = await _context.VehicleWarranties.FirstOrDefaultAsync(w => w.WarrantyId == warrantyId);

//        if (policy == null || warranty == null)
//        {
//            return NotFound("Policy or Warranty not found.");
//        }

//        // Create a view model with the selected product details
//        var product = new InsuranceProductViewModel
//        {
//            PolicyTypeId = policy.PolicyTypeId,
//            PolicyName = policy.PolicyName,
//            PolicyDetails = policy.PolicyDetails,
//            WarrantyId = warranty.WarrantyId,
//            WarrantyType = warranty.WarrantyType,
//            WarrantyDuration = warranty.WarrantyDuration,
//            WarrantyDetails = warranty.WarrantyDetails,
//         //   VehicleRate = (float)policy.VehicleRate
//        };

//        return View(product); // Return the Buy view with the selected product
//    }

//    [Route("confirmproduct")]
//    [HttpPost]
//    [ValidateAntiForgeryToken]
//    public async Task<IActionResult> Buy(int policyTypeId, int warrantyId, bool confirm = true)
//    {
//        // Fetch the selected policy and warranty from the database
//        var policy = await _context.VehiclePolicyTypes.FirstOrDefaultAsync(p => p.PolicyTypeId == policyTypeId);
//        var warranty = await _context.VehicleWarranties.FirstOrDefaultAsync(w => w.WarrantyId == warrantyId);

        if (policy == null || warranty == null)
        {
            return NotFound("Policy or Warranty not found.");
        }
        float adjustedRate = (float)policy.VehicleRate;

        if (warranty.WarrantyDuration.Contains("3"))
        {
            adjustedRate = adjustedRate;  // Add $50 for 5-year warranties
        }
        if (warranty.WarrantyDuration.Contains("5"))
        {
            adjustedRate += 50;  // Add $50 for 5-year warranties
        }
        else if (warranty.WarrantyDuration.Contains("7"))
        {
            adjustedRate += 100; // Add $100 for 7-year warranties
        }
        // Create a session object to store the selected product
        var productSession = new InsuranceProductViewModel
        {
            PolicyTypeId = policy.PolicyTypeId,
            PolicyName = policy.PolicyName,
            PolicyDetails = policy.PolicyDetails,
            WarrantyId = warranty.WarrantyId,
            WarrantyType = warranty.WarrantyType,
            WarrantyDuration = warranty.WarrantyDuration,
            WarrantyDetails = warranty.WarrantyDetails,
            VehicleRate = (float)adjustedRate
        };

//        // Save the productSession into the session
//        HttpContext.Session.SetObject("productSession", productSession);

//        // Log the session data to console
//        var sessionData = HttpContext.Session.GetObject<InsuranceProductViewModel>("productSession");
//        _logger.LogInformation("Session Confirmation: {@SessionData}", sessionData);
//        // Redirect to confirmation page
//        return RedirectToAction("Confirmation");
//    }

//    [Route("Confirmation")]
//    [HttpGet]
//    public IActionResult Confirmation()
//    {
//        // Retrieve the productSession if it exists
//        var productSession = HttpContext.Session.GetObject<InsuranceProductViewModel>("productSession");

//        if (productSession == null)
//        {
//            return RedirectToAction("Index"); // Redirect to the product listing if no session is found
//        }

//        // Check if the user is authenticated
//        if (!User.Identity.IsAuthenticated)
//        {
//            // If the user is not authenticated, redirect them to the login page
//            return RedirectToAction("Login", "Account");
//        }

//        return View(productSession); // Pass the session data to the view for display
//    }
//}
