using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Project3.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Services
{
    public class CarService
    {
        private readonly string _filePath;

        public CarService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.WebRootPath, "cars.json");
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Car>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<Car>>(json) ?? new List<Car>();
        }

        public async Task SaveCarsAsync(List<Car> cars)
        {
            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<List<VehicleInformation>> GetAllVehicleInformationsAsync()
        {
            var cars = await GetAllCarsAsync();
            var vehicleList = cars.SelectMany(car => car.Models.Select(model => new VehicleInformation
            {
                VehicleName = car.Name,
                VehicleModel = model.Name,
                VehicleVersion = model.Codename,
                // Note: JSON does not provide fields for VehicleRate, VehicleOwnerName, etc.
                // You might need to adjust this based on your actual data sources
            })).ToList();

            return vehicleList;
        }

        public async Task<VehicleInformation> GetVehicleInformationById(int id)
        {
            var vehicles = await GetAllVehicleInformationsAsync();
            return vehicles.FirstOrDefault(v => v.Id == id);
        }
    }
}
