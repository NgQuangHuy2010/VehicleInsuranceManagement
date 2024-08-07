using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Controllers
{
    [Route("[controller]/[action]")]
    public class InsuranceProcessController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;

        public InsuranceProcessController(VehicleInsuranceManagementContext context)
        {
            _context = context;
        }

        // GET: InsuranceProcess/Create
        [HttpGet]
        public IActionResult Create(string customerId, string customerName, string customerPhoneNumber, int vehicleId, string vehicleName, string vehicleModel, decimal vehicleRate, int? warrantyId, int policyTypeId)
        {
            var insuranceProcess = new InsuranceProcess
            {
                CustomerId = customerId,
                CustomerName = customerName,
                CustomerPhoneNumber = customerPhoneNumber,
                VehicleId = vehicleId,
                VehicleName = vehicleName,
                VehicleModel = vehicleModel,
                VehicleRate = vehicleRate,
                WarrantyId = warrantyId,
                PolicyTypeId = policyTypeId,
                PolicyDate = DateTime.Now.ToString("yyyy-MM-dd"),
                PolicyDuration = 12 // Default duration, can be adjusted
            };

            return View(insuranceProcess);
        }

        // POST: InsuranceProcess/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsuranceProcess insuranceProcess)
        {
            if (ModelState.IsValid)
            {
                _context.InsuranceProcesses.Add(insuranceProcess);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(insuranceProcess);
        }

        // GET: InsuranceProcess/Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var insuranceProcesses = await _context.InsuranceProcesses
                .Include(ip => ip.Vehicle)
                .Include(ip => ip.Warranty)
                .Include(ip => ip.PolicyType)
                .ToListAsync();
            return View(insuranceProcesses);
        }

        // GET: InsuranceProcess/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var insuranceProcess = await _context.InsuranceProcesses
                .Include(ip => ip.Vehicle)
                .Include(ip => ip.Warranty)
                .Include(ip => ip.PolicyType)
                .FirstOrDefaultAsync(ip => ip.Id == id);

            if (insuranceProcess == null)
            {
                return NotFound();
            }

            return View(insuranceProcess);
        }

        // GET: InsuranceProcess/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var insuranceProcess = await _context.InsuranceProcesses.FindAsync(id);
            if (insuranceProcess == null)
            {
                return NotFound();
            }

            return View(insuranceProcess);
        }

        // POST: InsuranceProcess/Edit/5
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,CustomerName,CustomerAdd,CustomerPhoneNumber,PolicyNumber,PolicyDate,PolicyDuration,VehicleNumber,VehicleName,VehicleModel,VehicleVersion,VehicleRate,VehicleWarranty,VehicleBodyNumber,VehicleEngineNumber,CustomerAddProve,VehicleId,WarrantyId,PolicyTypeId")] InsuranceProcess insuranceProcess)
        {
            if (id != insuranceProcess.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuranceProcess);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceProcessExists(insuranceProcess.Id))
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

            return View(insuranceProcess);
        }

        // GET: InsuranceProcess/Delete/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var insuranceProcess = await _context.InsuranceProcesses
                .Include(ip => ip.Vehicle)
                .Include(ip => ip.Warranty)
                .Include(ip => ip.PolicyType)
                .FirstOrDefaultAsync(ip => ip.Id == id);

            if (insuranceProcess == null)
            {
                return NotFound();
            }

            return View(insuranceProcess);
        }

        // POST: InsuranceProcess/Delete/5
        [HttpPost("{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var insuranceProcess = await _context.InsuranceProcesses.FindAsync(id);
            _context.InsuranceProcesses.Remove(insuranceProcess);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceProcessExists(int id)
        {
            return _context.InsuranceProcesses.Any(e => e.Id == id);
        }
    }
}
