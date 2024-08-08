//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Project3.Models;
//using Project3.Services;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Project3.Controllers
//{
//    [Route("VehicleInformations")]
//    public class VehicleInformationsController : Controller
//    {
//        private readonly CarService _carService;
//        private readonly VehicleInsuranceManagementContext _context;

//        public VehicleInformationsController(CarService carService, VehicleInsuranceManagementContext context)
//        {
//            _carService = carService;
//            _context = context;
//        }

//        //GET: VehicleInformations/CarSelection
//        [Route("CarSelection")]

//        public async Task<IActionResult> CarSelection()
//        {
//            var cars = await _carService.GetAllCarsAsync();
//            ViewBag.Manufacturers = cars.Select(c => new { c.Code, c.Name }).ToList();
//            return View();
//        }


//        [HttpPost]
//        [Route("LoadData")]
//        public async Task<IActionResult> LoadData()
//        {
//            var cars = await _carService.GetAllCarsAsync();
//            await _carService.SaveCarsAsync(cars);
//            return RedirectToAction(nameof(CarSelection));
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [HttpPost("SaveVehicleInformation")]
//        public async Task<IActionResult> SaveVehicleInformation([FromForm] VehicleInformation vehicleInformation)
//        {
//            if (ModelState.IsValid)
//            {
//                var vehicleList = await _carService.GetAllVehicleInformationsAsync();
//                // Remove manual setting of Id property
//                // vehicleInformation.Id = vehicleList.Count > 0 ? vehicleList.Max(c => c.Id) + 1 : 1;

//                vehicleList.Add(vehicleInformation);
//                await SaveVehicleInformationsAsync(vehicleList);

//                // Save into vehicle_information table
//                _context.VehicleInformations.Add(vehicleInformation);
//                await _context.SaveChangesAsync();




//                //var vehicleList = await _carService.GetAllVehicleInformationsAsync();
//                //vehicleInformation.Id = vehicleList.Count > 0 ? vehicleList.Max(c => c.Id) + 1 : 1;
//                //vehicleList.Add(vehicleInformation);
//                //await SaveVehicleInformationsAsync(vehicleList);

//                ////save into vehicleinfor
//                //_context.VehicleInformations.Add(vehicleInformation);
//                //await _context.SaveChangesAsync();

//                // Redirect to the Estimate form with vehicle information as query parameters
//                return RedirectToAction("Create", "Estimate", new
//                {
//                    vehicleInformation.VehicleName,
//                    vehicleInformation.VehicleModel,
//                    vehicleInformation.VehicleVersion,
//                    vehicleInformation.VehicleRate,
//                    vehicleInformation.VehicleBodyNumber,
//                    vehicleInformation.VehicleEngineNumber,
//                    vehicleInformation.VehicleNumber,
//                    vehicleInformation.Usage,
//                    vehicleInformation.Location,
//                    vehicleInformation.WarrantyType,
//                    vehicleInformation.PolicyType
//                });
//            }
//            return BadRequest(ModelState);
//        }


//        [Route("edit")]
//        public async Task<IActionResult> Edit(int id)
//        {
//            var vehicleInformation = await _carService.GetVehicleInformationById(id);
//            if (vehicleInformation == null)
//            {
//                return NotFound();
//            }

//            var cars = await _carService.GetAllCarsAsync();
//            ViewBag.Manufacturers = cars.Select(c => new { c.Code, c.Name }).Distinct().ToList();
//            return View(vehicleInformation);
//        }

//        [Route("edit")]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleName,VehicleOwnerName,VehicleModel,VehicleVersion,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleNumber,Location,Usage,WarrantyType,PolicyType")] VehicleInformation vehicleInformation)
//        {
//            if (id != vehicleInformation.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                var vehicleList = await _carService.GetAllVehicleInformationsAsync();
//                var vehicleToUpdate = vehicleList.FirstOrDefault(v => v.Id == id);
//                if (vehicleToUpdate != null)
//                {
//                    vehicleToUpdate.VehicleName = vehicleInformation.VehicleName;
//                    vehicleToUpdate.VehicleOwnerName = vehicleInformation.VehicleOwnerName;
//                    vehicleToUpdate.VehicleModel = vehicleInformation.VehicleModel;
//                    vehicleToUpdate.VehicleVersion = vehicleInformation.VehicleVersion;
//                    vehicleToUpdate.VehicleRate = vehicleInformation.VehicleRate;
//                    vehicleToUpdate.VehicleBodyNumber = vehicleInformation.VehicleBodyNumber;
//                    vehicleToUpdate.VehicleEngineNumber = vehicleInformation.VehicleEngineNumber;
//                    vehicleToUpdate.VehicleNumber = vehicleInformation.VehicleNumber;
//                    vehicleToUpdate.Location = vehicleInformation.Location;
//                    vehicleToUpdate.Usage = vehicleInformation.Usage;
//                    vehicleToUpdate.WarrantyType = vehicleInformation.WarrantyType;
//                    vehicleToUpdate.PolicyType = vehicleInformation.PolicyType;

//                    await SaveVehicleInformationsAsync(vehicleList);
//                    return RedirectToAction(nameof(Index));
//                }
//                return NotFound();
//            }
//            return View(vehicleInformation);
//        }

//        [Route("delete")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var vehicleInformation = await _carService.GetVehicleInformationById(id);
//            if (vehicleInformation == null)
//            {
//                return NotFound();
//            }
//            return View(vehicleInformation);
//        }

//        // POST: VehicleInformations/Delete/5
//        [Route("delete")]
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var vehicleList = await _carService.GetAllVehicleInformationsAsync();
//            var vehicleToDelete = vehicleList.FirstOrDefault(v => v.Id == id);
//            if (vehicleToDelete != null)
//            {
//                vehicleList.Remove(vehicleToDelete);
//                await SaveVehicleInformationsAsync(vehicleList);
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: VehicleInformations
//        [Route("index")]
//        public async Task<IActionResult> Index()
//        {
//            //var vehicleList = await _carService.GetAllVehicleInformationsAsync();
//            var vehicleList = await _context.VehicleInformations.ToListAsync();
//            return View(vehicleList);
//        }

//                private async Task SaveVehicleInformationsAsync(List<VehicleInformation> vehicleInformations)
//        {
//            var cars = vehicleInformations.GroupBy(v => v.VehicleName).Select(g => new Car
//            {
//                Name = g.Key,
//                Models = g.Select(v => new Model
//                {
//                    Name = v.VehicleModel,
//                    Codename = v.VehicleVersion,
//                    Price = v.VehicleRate.ToString()
//                }).ToList()
//            }).ToList();

//            await _carService.SaveCarsAsync(cars);
//        }
//    }
//}
