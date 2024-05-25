using ApiFinanzauto.Dtos;
using DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

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
        [Route("{id_client}/all")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles(int id_client)
        {
            var vehicles = await _context.Vehicles.Where(v => v.ClientId == id_client).ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet]
        // [Authorize]
        [Route("{id_client}/by_id/{id_vehicle}")]
        public async Task<IActionResult> GetVehicle(int id_client, int id_vehicle)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id_vehicle && v.ClientId == id_client);

            if (vehicle is null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpGet]
        [Route("test_split")]
        public ActionResult<IEnumerable<string>> GetTestSplit()
        {
            string texto = "Hola,mundo,amigo";
            string[] subcadenas = texto.Split(',');
            return Ok(subcadenas);
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
        [Route("{id_client}/{id_vehicle}")]
        public async Task<IActionResult> UpdateVehicle(int id_client, int id_vehicle, VehicleDto vehicle)
        {
            var vehicleCurrent = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id_vehicle && v.ClientId == id_client);
            if (vehicleCurrent is null)
            {
                return NotFound();
            }

            vehicleCurrent!.Name = vehicle.Name;
            vehicleCurrent.Plate = vehicle.Plate;
            vehicleCurrent.Color = vehicle.Color;
            vehicleCurrent.Brand = vehicle.Brand;
            vehicleCurrent.Line = vehicle.Line;
            vehicleCurrent.Year = vehicle.Year;
            vehicleCurrent.Kilimetres = vehicle.Kilimetres;
            vehicleCurrent.Cost = vehicle.Cost;
            vehicleCurrent.Image = vehicle.Image is not null ? vehicle.Image : "";
            vehicleCurrent.ClientId = vehicle.ClientId;
            vehicleCurrent.Observations = vehicle.Observations;
            vehicleCurrent.Status = vehicle.Status;
            vehicleCurrent.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        // [Authorize]
        [Route("{id_client}/{id_vehicle}")]
        public async Task<IActionResult> DeleteVehicle(int id_client, int id_vehicle)
        {
            var vehicleRemove = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id_vehicle && v.ClientId == id_client);
            if (vehicleRemove is null)
            {
                return NotFound();
            }

            if (vehicleRemove.Image is not null)
            {
                string[] paths = vehicleRemove.Image.Split(',');
                foreach (var path in paths)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
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

        [HttpDelete]
        // [Authorize]
        [Route("upload/{path}")]
        public IActionResult DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return Ok();
            }

            return NotFound();
        }
    }
}
