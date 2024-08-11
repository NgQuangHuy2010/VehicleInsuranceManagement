using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Areas.System.Controllers
{
    [Area("System")]
    [Route("System/InsuranceProduct")]
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
            // Fetch all policy types and warranties separately
            var policyTypes = await _context.VehiclePolicyTypes.ToListAsync();
            var warranties = await _context.VehicleWarranties.ToListAsync();

            // Combine the data into a list of view models
            var insuranceProducts = policyTypes.Select(policy => new InsuranceProductViewModel
            {
                PolicyTypeId = policy.PolicyTypeId,
                PolicyName = policy.PolicyName,
                PolicyDetails = policy.PolicyDetails,
                VehicleRate = (float)(policy.VehicleRate ?? 0),
                WarrantyId = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policy.PolicyName))?.WarrantyId ?? 0,
                WarrantyType = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policy.PolicyName))?.WarrantyType,
                WarrantyDuration = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policy.PolicyName))?.WarrantyDuration,
                WarrantyDetails = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policy.PolicyName))?.WarrantyDetails
            }).ToList();

            return View(insuranceProducts);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new InsuranceProductViewModel());
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsuranceProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var policyType = new VehiclePolicyType
                {
                    PolicyName = viewModel.PolicyName,
                    PolicyDetails = viewModel.PolicyDetails,
                    VehicleRate = viewModel.VehicleRate
                };

                _context.VehiclePolicyTypes.Add(policyType);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (policyType == null)
            {
                return NotFound();
            }

            // Fetch all warranties (you can apply more specific logic as needed)
            var warranties = await _context.VehicleWarranties.ToListAsync();

            var viewModel = new InsuranceProductViewModel
            {
                PolicyTypeId = policyType.PolicyTypeId,
                PolicyName = policyType.PolicyName,
                PolicyDetails = policyType.PolicyDetails,
                WarrantyId = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policyType.PolicyName))?.WarrantyId ?? 0,
                WarrantyType = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policyType.PolicyName))?.WarrantyType,
                WarrantyDuration = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policyType.PolicyName))?.WarrantyDuration,
                WarrantyDetails = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policyType.PolicyName))?.WarrantyDetails,
                VehicleRate = (float)policyType.VehicleRate
            };

            return View(viewModel);
        }

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
                var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
                if (policyType == null)
                {
                    return NotFound();
                }

                policyType.PolicyName = viewModel.PolicyName;
                policyType.PolicyDetails = viewModel.PolicyDetails;
                policyType.VehicleRate = viewModel.VehicleRate;

                _context.Update(policyType);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost("Delete/{id}")]
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

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            // Find the policy type by its ID
            var policyType = await _context.VehiclePolicyTypes.FindAsync(id);
            if (policyType == null)
            {
                return NotFound();
            }

            // Fetch the associated warranty details (or all warranties, if needed)
            var warranties = await _context.VehicleWarranties.ToListAsync();
            var matchingWarranty = warranties.FirstOrDefault(w => w.WarrantyType.Contains(policyType.PolicyName));

            // Create a view model to pass the details to the view
            var viewModel = new InsuranceProductViewModel
            {
                PolicyTypeId = policyType.PolicyTypeId,
                PolicyName = policyType.PolicyName,
                PolicyDetails = policyType.PolicyDetails,
                WarrantyId = matchingWarranty?.WarrantyId ?? 0,
                WarrantyType = matchingWarranty?.WarrantyType,
                WarrantyDuration = matchingWarranty?.WarrantyDuration,
                WarrantyDetails = matchingWarranty?.WarrantyDetails,
                VehicleRate = (float)(policyType.VehicleRate ?? 0)
            };

            // Pass the view model to the view
            return View(viewModel);
        }

        private bool PolicyTypeExists(int id)
        {
            return _context.VehiclePolicyTypes.Any(e => e.PolicyTypeId == id);
        }
    }
}
