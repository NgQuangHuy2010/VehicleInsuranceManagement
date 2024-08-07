using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;
using System.Threading.Tasks;

namespace Project3.Controllers
{
    public class CompanyBillingPolicyController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public CompanyBillingPolicyController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanyBillingPolicies.ToListAsync());
        }
        [Route("details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingPolicy = await _context.CompanyBillingPolicies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billingPolicy == null)
            {
                return NotFound();
            }

            return View(billingPolicy);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create(string customerName, decimal customerPhoneNumber, string vehicleName, string vehicleModel, decimal vehicleRate, string vehicleBodyNumber, string vehicleEngineNumber, decimal estimatedCost)
        {
            var model = new CompanyBillingPolicyViewModel
            {
                CustomerName = customerName,
                CustomerPhoneNumber = customerPhoneNumber,
                VehicleName = vehicleName,
                VehicleModel = vehicleModel,
                VehicleRate = vehicleRate,
                VehicleBodyNumber = vehicleBodyNumber,
                VehicleEngineNumber = vehicleEngineNumber,
                EstimatedCost = estimatedCost
            };

            return View(model);
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompanyBillingPolicyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var companyBillingPolicy = new CompanyBillingPolicy
                {
                    CustomerName = model.CustomerName,
                    CustomerPhoneNumber = model.CustomerPhoneNumber,
                    VehicleName = model.VehicleName,
                    VehicleModel = model.VehicleModel,
                    VehicleRate = model.VehicleRate,
                    VehicleBodyNumber = model.VehicleBodyNumber,
                    VehicleEngineNumber = model.VehicleEngineNumber,
                    Date = DateTime.Now,
                    Amount = Math.Round(model.EstimatedCost, 2)
                };

                _context.CompanyBillingPolicies.Add(companyBillingPolicy);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Payment"); // Redirect to the payment page or any other page you have
            }
            return View(model);
        }

        [Route("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingPolicy = await _context.CompanyBillingPolicies.FindAsync(id);
            if (billingPolicy == null)
            {
                return NotFound();
            }
            return View(billingPolicy);
        }
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,CustomerName,PolicyNumber,CustomerAddProve,CustomerPhoneNumber,BillNo,VehicleName,VehicleModel,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,Date,Amount")] CompanyBillingPolicy billingPolicy)
        {
            if (id != billingPolicy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billingPolicy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingPolicyExists(billingPolicy.Id))
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
            return View(billingPolicy);
        }
        [Route("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billingPolicy = await _context.CompanyBillingPolicies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billingPolicy == null)
            {
                return NotFound();
            }

            return View(billingPolicy);
        }
        [Route("delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billingPolicy = await _context.CompanyBillingPolicies.FindAsync(id);
            _context.CompanyBillingPolicies.Remove(billingPolicy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingPolicyExists(int id)
        {
            return _context.CompanyBillingPolicies.Any(e => e.Id == id);
        }
    }
}
