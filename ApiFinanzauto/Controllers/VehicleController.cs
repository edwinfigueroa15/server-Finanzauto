using ApiFinanzauto.Dtos;
using DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFinanzauto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly FinanzautoDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VehicleController(FinanzautoDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        // [Authorize]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet]
        // [Authorize]
        [Route("id")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpPost]
        // [Authorize]
        [Route("")]
        public async Task<IActionResult> Createvehicle(VehicleDto vehicle)
        {
            Vehicle vehicleNew = new Vehicle();
            vehicleNew.Name = vehicle.Name;
            vehicleNew.Plate = vehicle.Plate;
            vehicleNew.Color = vehicle.Color;
            vehicleNew.Brand = vehicle.Brand;
            vehicleNew.Line = vehicle.Line;
            vehicleNew.Year = vehicle.Year;
            vehicleNew.Kilimetres = vehicle.Kilimetres;
            vehicleNew.Cost = vehicle.Cost;
            vehicleNew.Image = vehicle.Image;
            vehicleNew.ClientId = vehicle.ClientId;
            vehicleNew.Status = vehicle.Status;
            vehicleNew.Observations = vehicle.Observations;
            vehicleNew.CreatedAt = DateTime.Now;
            vehicleNew.UpdatedAt = DateTime.Now;

            await _context.Vehicles.AddAsync(vehicleNew);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        // [Authorize]
        [Route("")]
        public async Task<IActionResult> UpdateVehicle(int id, VehicleDto vehicle)
        {
            var vehicleCurrent = await _context.Vehicles.FindAsync(id);
            vehicleCurrent!.Name = vehicle.Name;
            vehicleCurrent.Plate = vehicle.Plate;
            vehicleCurrent.Color = vehicle.Color;
            vehicleCurrent.Brand = vehicle.Brand;
            vehicleCurrent.Line = vehicle.Line;
            vehicleCurrent.Year = vehicle.Year;
            vehicleCurrent.Kilimetres = vehicle.Kilimetres;
            vehicleCurrent.Cost = vehicle.Cost;
            vehicleCurrent.Image = vehicle.Image;
            vehicleCurrent.ClientId = vehicle.ClientId;
            vehicleCurrent.Observations = vehicle.Observations;
            vehicleCurrent.Status = vehicle.Status;
            vehicleCurrent.UpdatedAt = DateTime.Now;

            if (vehicleCurrent.Image is not null && vehicle.Image is not null)
            {
                if (vehicleCurrent.Image != vehicle.Image)
                {
                    if (System.IO.File.Exists(vehicleCurrent.Image))
                    {
                        System.IO.File.Delete(vehicleCurrent.Image);
                    }

                    vehicleCurrent.Image = vehicle.Image;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        // [Authorize]
        [Route("")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicleRemove = await _context.Vehicles.FindAsync(id);
            if (vehicleRemove is null)
            {
                return NotFound();
            }

            if (vehicleRemove.Image is not null && System.IO.File.Exists(vehicleRemove.Image))
            {
                System.IO.File.Delete(vehicleRemove.Image);
            }

            _context.Vehicles.Remove(vehicleRemove);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        // [Authorize]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            var date = DateTime.Now.GetHashCode();
            try
            {
                if (file.Length == 0) return BadRequest();

                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fullPath = Path.Combine(path, date + file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(fullPath);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
