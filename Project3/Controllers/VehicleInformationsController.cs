using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project3.Models;
using Project3.ModelsView;
using Project3.ModelsView.Identity;
using Project3.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Project3.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class VehicleInformationsController : Controller
    {
        private readonly CarService _carService;
        private readonly VehicleInsuranceManagementContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EstimatesController> _logger;
        public VehicleInformationsController(ILogger<EstimatesController> logger,UserManager<ApplicationUser> userManager, CarService carService, VehicleInsuranceManagementContext context)
        {
            _userManager = userManager;
            _carService = carService;
            _context = context;
            _logger = logger;
        }

        GET: VehicleInformations/CarSelection
        [HttpGet("CarSelection")]

        public async Task<IActionResult> CarSelection()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");  // Redirect to a login page if user is not authenticated
            }

            // Retrieve the product session
            var productSession = HttpContext.Session.GetObject<InsuranceProductViewModel>("productSession");
            if (productSession == null)
            {
                return RedirectToAction("Index", "InsuranceProducts"); // If no product is selected, redirect to product selection
            }


            var cars = await _carService.GetAllCarsAsync();
            ViewBag.Manufacturers = cars.Select(c => new { c.Code, c.Name }).ToList();
            ViewBag.CustomerId = user.Id;
            ViewBag.CustomerName = user.Fullname;
            ViewBag.CustomerPhoneNumber = user.PhoneNumber;
            return View();
        }


                [HttpPost]
                [Route("LoadData")]
                public async Task<IActionResult> LoadData()
                {
                    var cars = await _carService.GetAllCarsAsync();
                    await _carService.SaveCarsAsync(cars);
                    return RedirectToAction(nameof(CarSelection));
                }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SaveVehicleInformation")]
        public async Task<IActionResult> SaveVehicleInformation([FromForm] VehicleInformationViewModel vehicleInformation)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }
            if (ModelState.IsValid)
            {
                // Save vehicle information data into session
                HttpContext.Session.SetObject("VehicleInformationData", vehicleInformation);

                // Log the session data to console
                var sessionData = HttpContext.Session.GetObject<VehicleInformation>("VehicleInformationData");
                _logger.LogInformation("Session Vehicle: {@SessionData}", sessionData);

                 Redirect to the Estimate form with vehicle information as query parameters
                return RedirectToAction("Create", "Estimates", new
                {
                    VehicleId = vehicleInformation.Id,
                    vehicleName = vehicleInformation.VehicleName,
                    vehicleModel = vehicleInformation.VehicleModel,
                    vehicleVersion = vehicleInformation.VehicleVersion,
                    vehicleRate = vehicleInformation.VehicleRate,
                    customerId = user.Id,
                    customerName = user.Fullname,
                    customerPhoneNumber = user.Phone
                });
            }
            return BadRequest(ModelState);
        }

        [Route("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleInformation = await _carService.GetVehicleInformationById(id);
            if (vehicleInformation == null)
            {
                return NotFound();
            }

                        var cars = await _carService.GetAllCarsAsync();
                        ViewBag.Manufacturers = cars.Select(c => new { c.Code, c.Name }).Distinct().ToList();
                        return View(vehicleInformation);
                    }

                    [Route("edit")]
                    [HttpPost]
                    [ValidateAntiForgeryToken]
                    public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleName,VehicleOwnerName,VehicleModel,VehicleVersion,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleNumber,Location,Usage,WarrantyType,PolicyType")] VehicleInformation vehicleInformation)
                    {
                        if (id != vehicleInformation.Id)
                        {
                            return NotFound();
                        }

            if (ModelState.IsValid)
            {
                var vehicleList = await _carService.GetAllVehicleInformationsAsync();
                var vehicleToUpdate = vehicleList.FirstOrDefault(v => v.Id == id);
                if (vehicleToUpdate != null)
                {
                    vehicleToUpdate.VehicleName = vehicleInformation.VehicleName;
                    vehicleToUpdate.VehicleOwnerName = vehicleInformation.VehicleOwnerName;
                    vehicleToUpdate.VehicleModel = vehicleInformation.VehicleModel;
                    vehicleToUpdate.VehicleVersion = vehicleInformation.VehicleVersion;
                    vehicleToUpdate.VehicleRate = vehicleInformation.VehicleRate;
                    vehicleToUpdate.VehicleBodyNumber = vehicleInformation.VehicleBodyNumber;
                    vehicleToUpdate.VehicleEngineNumber = vehicleInformation.VehicleEngineNumber;
                    vehicleToUpdate.VehicleNumber = vehicleInformation.VehicleNumber;


                                        await SaveVehicleInformationsAsync(vehicleList);
                                        return RedirectToAction(nameof(Index));
                                    }
                                    return NotFound();
                                }
                                return View(vehicleInformation);
                            }

                            [Route("delete")]
                            public async Task<IActionResult> Delete(int id)
                            {
                                var vehicleInformation = await _carService.GetVehicleInformationById(id);
                                if (vehicleInformation == null)
                                {
                                    return NotFound();
                                }
                                return View(vehicleInformation);
                            }

                            // POST: VehicleInformations/Delete/5
                            [Route("delete")]
                            [HttpPost]
                            [ValidateAntiForgeryToken]
                            public async Task<IActionResult> DeleteConfirmed(int id)
                            {
                                var vehicleList = await _carService.GetAllVehicleInformationsAsync();
                                var vehicleToDelete = vehicleList.FirstOrDefault(v => v.Id == id);
                                if (vehicleToDelete != null)
                                {
                                    vehicleList.Remove(vehicleToDelete);
                                    await SaveVehicleInformationsAsync(vehicleList);
                                }
                                return RedirectToAction(nameof(Index));
                            }

                            // GET: VehicleInformations
                            [Route("index")]
                            public async Task<IActionResult> Index()
                            {
                                //var vehicleList = await _carService.GetAllVehicleInformationsAsync();
                                var vehicleList = await _context.VehicleInformations.ToListAsync();
                                return View(vehicleList);
                            }

                    private async Task SaveVehicleInformationsAsync(List<VehicleInformation> vehicleInformations)
                    {
                        var cars = vehicleInformations.GroupBy(v => v.VehicleName).Select(g => new Car
                        {
                            Name = g.Key,
                            Models = g.Select(v => new Model
                            {
                                Name = v.VehicleModel,
                                Codename = v.VehicleVersion,
                                Price = v.VehicleRate.ToString()
                            }).ToList()
                        }).ToList();

            await _carService.SaveCarsAsync(cars);
        }
    }
}
