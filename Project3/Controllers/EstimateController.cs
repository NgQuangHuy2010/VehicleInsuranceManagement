using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using System.Linq;
using System.Threading.Tasks;
using Project3.ModelsView;
using Microsoft.AspNetCore.Identity;
using Project3.ModelsView.Identity;
using Humanizer;
namespace Project3.Controllers
{
    [Route("Estimates")]
    public class EstimatesController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EstimatesController> _logger;
        public EstimatesController(UserManager<ApplicationUser> userManager, VehicleInsuranceManagementContext context, ILogger<EstimatesController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // GET: Estimates/Create
        //[HttpGet("Create")]
        //public IActionResult Create(int vehicleId, string vehicleName, string vehicleModel, decimal vehicleRate, string customerId, string customerName, string customerPhoneNumber)
        //{
        //    var vehicle = _context.VehicleInformations
        //        .FirstOrDefault(v => v.Id == vehicleId && v.VehicleName == vehicleName && v.VehicleModel == vehicleModel && v.VehicleRate == vehicleRate);

        //    if (vehicle == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new EstimateViewModel
        //    {
        //        VehicleId = vehicle.Id,
        //        VehicleName = vehicle.VehicleName,
        //        VehicleModel = vehicle.VehicleModel,
        //        VehicleRate = vehicle.VehicleRate,
        //        CustomerId = customerId,
        //        CustomerName = customerName,
        //        CustomerPhoneNumber = customerPhoneNumber,
        //        Warranties = _context.VehicleWarranties.Select(w => new SelectListItem
        //        {
        //            Value = w.WarrantyId.ToString(),
        //            Text = w.WarrantyType
        //        }).ToList(),
        //        PolicyTypes = _context.VehiclePolicyTypes.Select(p => new SelectListItem
        //        {
        //            Value = p.PolicyTypeId.ToString(),
        //            Text = p.PolicyName
        //        }).ToList()
        //    };

        //    return View(viewModel);
        //}
        [HttpGet("Create")]
        public async Task<IActionResult> Create() {
            
            var policytypename = await _context.VehiclePolicyTypes.ToListAsync();
            var warrantyname = await _context.VehicleWarranties.ToListAsync();
            var sessionData = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModel = new EstimateModelView
            {
                CustomerId = user.Id,
                CustomerName = user.Fullname,
                CustomerPhoneNumber = user.Phone,
                VehicleId = sessionData.Id,
                VehicleName = sessionData.VehicleName,
                VehicleModel = sessionData.VehicleModel,
                VehicleVersion = sessionData.VehicleVersion,
                VehicleRate = sessionData.VehicleRate,

                
            };
            ViewBag.policytype = policytypename;
            ViewBag.warranty = warrantyname;
            return View(viewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstimateModelView viewModel)
        {

            if (ModelState.IsValid)
            {
                
                try
                {
                    viewModel.Policies = await _context.VehiclePolicyTypes.AnyAsync(c => c.PolicyTypeId == viewModel.PolicyTypeId);
                    //var sessionVehicle = HttpContext.Session.GetObject<VehicleInformation>("VehicleInformationData");
                    var estimate = new EstimateModelView
                    {
                        CustomerId = viewModel.CustomerId,
                        EstimateNumber = viewModel.EstimateNumber,
                        CustomerName = viewModel.CustomerName,
                        CustomerPhoneNumber = viewModel.CustomerPhoneNumber,
                        VehicleName = viewModel.VehicleName,
                        VehicleModel = viewModel.VehicleModel,
                        VehicleVersion = viewModel.VehicleVersion, // Ensure VehicleVersion is assigned
                        VehicleRate = viewModel.VehicleRate,
                        VehicleId = viewModel.VehicleId,
                        PolicyTypeId = viewModel.PolicyTypeId,
                        WarrantyId = viewModel.WarrantyId

                    };
                    // Log successful creation
                    _logger.LogInformation("Estimate created successfully with ID {EstimateId}", estimate.EstimateNumber);

                    // Save estimate data into session
                    HttpContext.Session.SetObject("EstimateData", estimate);

                    // Log the session data to console
                    var sessionData = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
                    _logger.LogInformation("Session Estimate: {@SessionData}", sessionData);

                    // Redirect to InsuranceProcess Create view
                    return RedirectToAction("CollectInfo", "InsuranceProcess");
                }
                catch (Exception ex)
                {
                    // Log the error
                    _logger.LogError(ex, "Error occurred while creating estimate");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the estimate.");
                }
            }
            else
            {
                // Log validation errors
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Any())
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            _logger.LogWarning("ModelState Error: {Key} - {ErrorMessage}", state.Key, error.ErrorMessage);
                        }
                    }
                }
            }


            return View(viewModel);
        }
        // GET: Estimates/Details/5
        [HttpGet("Details/{id}")]
        //public async Task<IActionResult> Details(int id)
        //{
        //    //var estimate = await _context.Estimates
        //        ////.Include(e => e.Vehicle)
        //        ////.Include(e => e.Warranty)
        //        //.Include(e => e.PolicyType)
        //        //.FirstOrDefaultAsync(m => m.EstimateNumber == id);
        //    if (estimate == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(estimate);
        //}

        // GET: Estimates/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var estimate = await _context.Estimates.FindAsync(id);
            if (estimate == null)
            {
                return NotFound();
            }
            ViewBag.Warranties = new SelectList(_context.VehicleWarranties, "WarrantyId", "WarrantyType", estimate.WarrantyId);
            ViewBag.PolicyTypes = new SelectList(_context.VehiclePolicyTypes, "PolicyTypeId", "PolicyName", estimate.PolicyTypeId);
            return View(estimate);
        }

        // POST: Estimates/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,EstimateNumber,CustomerName,CustomerPhoneNumber,VehicleName,VehicleModel,VehicleRate,WarrantyId,PolicyTypeId,VehicleId")] Estimate estimate)
        {
            if (id != estimate.EstimateNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estimate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstimateExists(estimate.EstimateNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Warranties = new SelectList(_context.VehicleWarranties, "WarrantyId", "WarrantyType", estimate.WarrantyId);
            ViewBag.PolicyTypes = new SelectList(_context.VehiclePolicyTypes, "PolicyTypeId", "PolicyName", estimate.PolicyTypeId);
            return View(estimate);
        }

        // GET: Estimates/Delete/5
        [HttpGet("Delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{

        //    var estimate = await _context.Estimates
        //       // .Include(e => e.Vehicle)
        //        .Include(e => e.Warranty)
        //        .Include(e => e.PolicyType)
        //        .FirstOrDefaultAsync(m => m.EstimateNumber == id);
        //    if (estimate == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(estimate);
        //}

        // POST: Estimates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estimate = await _context.Estimates.FindAsync(id);
            _context.Estimates.Remove(estimate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstimateExists(int id)
        {
            return _context.Estimates.Any(e => e.EstimateNumber == id);
        }
    }
}
