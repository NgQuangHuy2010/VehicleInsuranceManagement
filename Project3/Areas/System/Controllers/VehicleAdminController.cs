using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3.Models;
using Project3.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Areas.Admin.Controllers
{
    [Area("system")]
    [Route("system/vehicleadmin")]
    public class VehicleInformationController : Controller
    {
        private readonly VehicleInsuranceManagementContext _context;
        private readonly CarService _carService;

        public VehicleInformationController(VehicleInsuranceManagementContext context, CarService carService)
        {
            _context = context;
            _carService = carService;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
            //var vehicleInformations = await _carService.GetAllVehicleInformationsAsync();
            //return View(vehicleInformations);
            // Fetch data from the database instead of the JSON file
            var vehicleList = await _context.VehicleInformations.ToListAsync();
            return View(vehicleList);
        }

        [Route("detail")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleInformation = await _carService.GetVehicleInformationById(id.Value);
            if (vehicleInformation == null)
            {
                return NotFound();
            }

            return View(vehicleInformation);
        }

        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleName,VehicleOwnerName,VehicleModel,VehicleVersion,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleNumber,Location,Usage,WarrantyType,PolicyType")] VehicleInformation vehicleInformation)
        {
            if (ModelState.IsValid)
            {
                var vehicleList = await _carService.GetAllVehicleInformationsAsync();
                vehicleList.Add(vehicleInformation);
                await SaveVehicleInformationsAsync(vehicleList);

                _context.VehicleInformations.Add(vehicleInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleInformation);
        }

        [Route("edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleInformation = await _carService.GetVehicleInformationById(id.Value);
            if (vehicleInformation == null)
            {
                return NotFound();
            }
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
                try
                {
                    _context.Update(vehicleInformation);
                    await _context.SaveChangesAsync();

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
                        vehicleToUpdate.Location = vehicleInformation.Location;
                        vehicleToUpdate.Usage = vehicleInformation.Usage;
                        vehicleToUpdate.WarrantyType = vehicleInformation.WarrantyType;
                        vehicleToUpdate.PolicyType = vehicleInformation.PolicyType;

                        await _carService.SaveCarsAsync(ConvertToCarList(vehicleList));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleInformationExists(vehicleInformation.Id))
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
            return View(vehicleInformation);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleInformation = await _carService.GetVehicleInformationById(id.Value);
            if (vehicleInformation == null)
            {
                return NotFound();
            }

            return View(vehicleInformation);
        }

        [Route("delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleInformation = await _context.VehicleInformations.FindAsync(id);
            _context.VehicleInformations.Remove(vehicleInformation);
            await _context.SaveChangesAsync();

            var vehicleList = await _carService.GetAllVehicleInformationsAsync();
            var vehicleToDelete = vehicleList.FirstOrDefault(v => v.Id == id);
            if (vehicleToDelete != null)
            {
                vehicleList.Remove(vehicleToDelete);
                await _carService.SaveCarsAsync(ConvertToCarList(vehicleList));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VehicleInformationExists(int id)
        {
            return _context.VehicleInformations.Any(e => e.Id == id);
        }

        private async Task SaveVehicleInformationsAsync(List<VehicleInformation> vehicleInformations)
        {
            var cars = ConvertToCarList(vehicleInformations);
            await _carService.SaveCarsAsync(cars);
        }

        private List<Car> ConvertToCarList(List<VehicleInformation> vehicleInformations)
        {
            // Implement the conversion logic from VehicleInformation to Car
            var cars = new List<Car>();

            var manufacturers = vehicleInformations.GroupBy(v => v.VehicleName);

            foreach (var manufacturerGroup in manufacturers)
            {
                var car = new Car
                {
                    Name = manufacturerGroup.Key,
                    Models = manufacturerGroup.Select(mg => new Model
                    {
                        Name = mg.VehicleModel,
                        Codename = mg.VehicleVersion,
                        Price = mg.VehicleRate.ToString(),
                        Trims = new List<Trim>() // Add trims if necessary
                    }).ToList()
                };

                cars.Add(car);
            }

            return cars;
        }
    }
}
