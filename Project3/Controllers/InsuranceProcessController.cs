using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.ModelsView;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Controllers
{
    
    public class InsuranceProcessController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;
        private readonly ILogger<EstimatesController> _logger;
        public InsuranceProcessController(ILogger<EstimatesController> logger, VehicleInsuranceManagementContext context)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet("CollectInfo")]
        public IActionResult CollectInfo()
        {
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            if (estimate == null)
            {
                return RedirectToAction("Index", "Home"); // or any appropriate action
            }
            var collectinfo = new CollectInfoViewModel
            {
                VehicleRate = estimate.VehicleRate,
            };
            return View(collectinfo);
        }

        [HttpPost("CollectInfo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CollectInfo(CollectInfoViewModel viewModel)
        {
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            if (estimate == null)
            {
                return RedirectToAction("Index", "Home"); // or any appropriate action
            }
            
            if (ModelState.IsValid)
            {
                var collectinfo = new CollectInfoViewModel
                {
                    VehicleRate = estimate.VehicleRate,
                    DriverAge = viewModel.DriverAge,
                    DrivingHistory = viewModel.DrivingHistory,
                    CustomerAdd = viewModel.CustomerAdd,
                    Usage  = viewModel.Usage,
                    AntiTheftDevice = viewModel.AntiTheftDevice,
                    MultiPolicy = viewModel.MultiPolicy,
                    SafeDriver = viewModel.SafeDriver,
                };
                // Store the viewModel data in session
                HttpContext.Session.SetObject("CollectInfoData", collectinfo);

                // Log the session data to console
                var sessionData = HttpContext.Session.GetObject<CollectInfoViewModel>("CollectInfoData");
                _logger.LogInformation("Session Insurance Input Data: {@SessionData}", sessionData);

                return RedirectToAction("Create", "InsuranceProcess");
            }

            return View(viewModel);
        }

        // GET: InsuranceProcess/Create
        [HttpGet]
        public IActionResult Create()
        {
            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");

            if (estimate == null)
            {
                return RedirectToAction("Index", "Home"); // or any appropriate action
            }

            var insuranceProcess = new InsuranceProcessViewModel
            {
                CustomerId = estimate.CustomerId,
                CustomerName = estimate.CustomerName,
                CustomerPhoneNumber = estimate.CustomerPhoneNumber,
                VehicleId = estimate.VehicleId,
                VehicleName = estimate.VehicleName,
                VehicleModel = estimate.VehicleModel,
                VehicleRate = estimate.VehicleRate,
                WarrantyId = estimate.WarrantyId,
                PolicyTypeId = estimate.PolicyTypeId,
                PolicyDate = DateTime.Now.ToString("yyyy-MM-dd"),
                PolicyDuration = 12, // Default duration, can be adjusted
                VehicleBodyNumber = vehicleinfo.VehicleBodyNumber,
                VehicleEngineNumber = vehicleinfo.VehicleEngineNumber
                  
            };

            return View(insuranceProcess);
        }


        // POST: InsuranceProcess/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsuranceProcessViewModel insuranceProcess)
        {
            Random random = new Random();
            string randomNumber = random.Next(10000000, 99999999).ToString();

            var estimate = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
            var vehicleinfo = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
            
            if (ModelState.IsValid)
            {
                var insurances = new InsuranceProcessViewModel
                {
                    CustomerId = insuranceProcess.CustomerId,
                    CustomerName = insuranceProcess.CustomerName,
                    CustomerPhoneNumber = insuranceProcess.CustomerPhoneNumber,
                    VehicleId = insuranceProcess.VehicleId,
                    VehicleName = insuranceProcess.VehicleName,
                    VehicleModel = insuranceProcess.VehicleModel,
                    VehicleRate = insuranceProcess.VehicleRate,
                    
                    WarrantyId = insuranceProcess.WarrantyId,
                    PolicyNumber = randomNumber,
                    PolicyTypeId = insuranceProcess.PolicyTypeId,
                    PolicyDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    PolicyDuration = 12, // Default duration, can be adjusted
                    VehicleBodyNumber = insuranceProcess.VehicleBodyNumber,
                    VehicleEngineNumber = insuranceProcess.VehicleEngineNumber
                };

                HttpContext.Session.SetObject("InsuranceData", insurances);

                // Log the session data to console
                var sessionData = HttpContext.Session.GetObject<InsuranceProcessViewModel>("InsuranceData");
                var sessionData1 = HttpContext.Session.GetObject<EstimateModelView>("EstimateData");
                var sessionData2 = HttpContext.Session.GetObject<VehicleInformationViewModel>("VehicleInformationData");
                

                _logger.LogInformation("Session Insurance Process: {@SessionData}", sessionData);
                _logger.LogInformation("Session Estimate: {@SessionData}", sessionData1);
                _logger.LogInformation("Session Vehicleinfo: {@SessionData}", sessionData2);
                


                return RedirectToAction("Create","CompanyBillingPolicy");
            }

            return View(insuranceProcess);
        }

        // GET: InsuranceProcess/Index
        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var insuranceProcesses = await _context.InsuranceProcesses
        //        .Include(ip => ip.Vehicle)
        //        .Include(ip => ip.Warranty)
        //        .Include(ip => ip.PolicyType)
        //        .ToListAsync();
        //    return View(insuranceProcesses);
        //}

        // GET: InsuranceProcess/Details/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var insuranceProcess = await _context.InsuranceProcesses
        //        .Include(ip => ip.Vehicle)
        //        .Include(ip => ip.Warranty)
        //        .Include(ip => ip.PolicyType)
        //        .FirstOrDefaultAsync(ip => ip.Id == id);

        //    if (insuranceProcess == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(insuranceProcess);
        //}

        //// GET: InsuranceProcess/Edit/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var insuranceProcess = await _context.InsuranceProcesses.FindAsync(id);
        //    if (insuranceProcess == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(insuranceProcess);
        //}

        //// POST: InsuranceProcess/Edit/5
        //[HttpPost("{id}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,CustomerName,CustomerAdd,CustomerPhoneNumber,PolicyNumber,PolicyDate,PolicyDuration,VehicleNumber,VehicleName,VehicleModel,VehicleVersion,VehicleRate,VehicleWarranty,VehicleBodyNumber,VehicleEngineNumber,CustomerAddProve,VehicleId,WarrantyId,PolicyTypeId")] InsuranceProcess insuranceProcess)
        //{
        //    if (id != insuranceProcess.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(insuranceProcess);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!InsuranceProcessExists(insuranceProcess.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(insuranceProcess);
        //}

        //// GET: InsuranceProcess/Delete/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var insuranceProcess = await _context.InsuranceProcesses
        //        .Include(ip => ip.Vehicle)
        //        .Include(ip => ip.Warranty)
        //        .Include(ip => ip.PolicyType)
        //        .FirstOrDefaultAsync(ip => ip.Id == id);

        //    if (insuranceProcess == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(insuranceProcess);
        //}

        //// POST: InsuranceProcess/Delete/5
        //[HttpPost("{id}"), ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var insuranceProcess = await _context.InsuranceProcesses.FindAsync(id);
        //    _context.InsuranceProcesses.Remove(insuranceProcess);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool InsuranceProcessExists(int id)
        //{
        //    return _context.InsuranceProcesses.Any(e => e.Id == id);
        //}
    }
}
