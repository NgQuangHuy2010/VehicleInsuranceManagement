using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;
namespace Project3.Areas.System.Controllers
{
    [Authorize(Policy = "AuthorizeSystemAreas")]
    [Area("system")]
    [Route("system/insuranceproduct")]
    public class AdminInsuranceProductsController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public AdminInsuranceProductsController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            // Fetch all policy types and warranties
            var policyTypes = await _context.VehiclePolicyTypes.ToListAsync();
            var warranties = await _context.VehicleWarranties.ToListAsync();

            // Combine the data into a list of view models
            var insuranceProducts = policyTypes.Select(policy =>
            {
                // Match warranties based on some other criteria
                var matchingWarranty = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policy.PolicyName));

                return new InsuranceProductViewModel
                {
                    PolicyTypeId = policy.PolicyTypeId,
                    PolicyName = policy.PolicyName,
                    PolicyDetails = policy.PolicyDetails,
                    //VehicleRate = (float)(policy.VehicleRate ?? 0),
                    WarrantyId = matchingWarranty?.WarrantyId ?? 0,
                    WarrantyType = matchingWarranty?.WarrantyType,
                    WarrantyDuration = matchingWarranty?.WarrantyDuration,
                    WarrantyDetails = matchingWarranty?.WarrantyDetails
                };
            }).ToList();

            return View(insuranceProducts);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            // Fetch all policy types and warranties from the database
            var policyTypes = await _context.VehiclePolicyTypes.ToListAsync();
            var warranties = await _context.VehicleWarranties.ToListAsync();

            // This could be replaced by a ViewModel to combine data from both tables
            var viewModel = new InsuranceProductViewModel
            {
                // Initialize the properties based on the fetched data
                // Adjust this according to how you want to display data
                // Example: Using SelectList for dropdowns if required in the view
                PolicyTypeId = policyTypes.FirstOrDefault()?.PolicyTypeId ?? 0,
                PolicyName = policyTypes.FirstOrDefault()?.PolicyName,
                PolicyDetails = policyTypes.FirstOrDefault()?.PolicyDetails,
                WarrantyId = warranties.FirstOrDefault()?.WarrantyId ?? 0,
                WarrantyType = warranties.FirstOrDefault()?.WarrantyType,
                WarrantyDuration = warranties.FirstOrDefault()?.WarrantyDuration,
                WarrantyDetails = warranties.FirstOrDefault()?.WarrantyDetails,
               // VehicleRate = (float)(policyTypes.FirstOrDefault()?.VehicleRate ?? 0)
            };

            return View(viewModel);
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsuranceProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    // Add logic to save the new policy or warranty, if that's the intent
                    var policyType = new VehiclePolicyType
                    {
                        PolicyName = viewModel.PolicyName,
                        PolicyDetails = viewModel.PolicyDetails,
                       // Vehicle = viewModel.VehicleRate
                    };

                    _context.VehiclePolicyTypes.Add(policyType);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
                }
            }

            // If the ModelState is invalid or an error occurs, re-fetch the data for the view
            var policyTypes = await _context.VehiclePolicyTypes.ToListAsync();
            var warranties = await _context.VehicleWarranties.ToListAsync();

            viewModel.PolicyTypeId = policyTypes.FirstOrDefault()?.PolicyTypeId ?? 0;
            viewModel.WarrantyId = warranties.FirstOrDefault()?.WarrantyId ?? 0;

            return View(viewModel);
        }
        // GET: System/InsuranceProducts/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the policy type by ID
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (policyType == null)
            {
                return NotFound();
            }

            // Fetch warranties (you might want to apply logic to pick the right one)
            var warranty = await _context.VehicleWarranties.FirstOrDefaultAsync();

            var viewModel = new InsuranceProductViewModel
            {
                PolicyTypeId = policyType.PolicyTypeId,
                PolicyName = policyType.PolicyName,
                PolicyDetails = policyType.PolicyDetails,
              //  VehicleRate = (float)(policyType.VehicleRate ?? 0),
                WarrantyId = warranty?.WarrantyId ?? 0,
                WarrantyType = warranty?.WarrantyType,
                WarrantyDuration = warranty?.WarrantyDuration,
                WarrantyDetails = warranty?.WarrantyDetails
            };

            return View(viewModel);
        }

        // GET: System/InsuranceProducts/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (policyType == null)
            {
                return NotFound();
            }

            var warranties = await _context.VehicleWarranties.ToListAsync();

            var viewModel = new InsuranceProductViewModel
            {
                PolicyTypeId = policyType.PolicyTypeId,
                PolicyName = policyType.PolicyName,
                PolicyDetails = policyType.PolicyDetails,
                WarrantyId = warranties.FirstOrDefault()?.WarrantyId ?? 0,
                WarrantyType = warranties.FirstOrDefault()?.WarrantyType,
                WarrantyDuration = warranties.FirstOrDefault()?.WarrantyDuration,
                WarrantyDetails = warranties.FirstOrDefault()?.WarrantyDetails,
              //  VehicleRate = (float)policyType.VehicleRate
            };

            return View(viewModel);
        }

        // POST: System/InsuranceProducts/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InsuranceProductViewModel viewModel)
        {
            if (id != viewModel.PolicyTypeId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
                    if (policyType == null)
                    {
                        return NotFound();
                    }

                    policyType.PolicyName = viewModel.PolicyName;
                    policyType.PolicyDetails = viewModel.PolicyDetails;
                   // policyType.VehicleRate = viewModel.VehicleRate;

                    _context.Update(policyType);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyTypeExists(viewModel.PolicyTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(viewModel);
        }

        // GET: System/InsuranceProducts/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var policyType = await _context.VehiclePolicyTypes
                .FirstOrDefaultAsync(m => m.PolicyTypeId == id);

            if (policyType == null)
            {
                return NotFound();
            }

            return View(policyType);
        }

        // POST: System/InsuranceProducts/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (policyType != null)
            {
                _context.VehiclePolicyTypes.Remove(policyType);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PolicyTypeExists(int id)
        {
            return _context.VehiclePolicyTypes.Any(e => e.PolicyTypeId == id);
        }
    }
}