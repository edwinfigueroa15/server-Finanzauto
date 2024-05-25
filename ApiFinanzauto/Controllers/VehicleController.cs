using ApiFinanzauto.Dtos;
using DB;
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
        [Route("{id_client}/all")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles(int id_client, string? search, int pageIndex, int pageSize)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            var vehiclesLength = _context.Vehicles.Where(v => v.ClientId == id_client).Count();

            if(search is not null)
            {
                if (pageIndex == 0 || pageSize == 0)
                {
                    vehicles = await _context.Vehicles.Where(v => v.Plate.Contains(search)).ToListAsync();
                }
                else
                {
                    vehicles = await _context.Vehicles.Where(v => v.ClientId == id_client && v.Plate.Contains(search)).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                }
            }
            else if (pageIndex == 0 || pageSize == 0)
            {
                vehicles = await _context.Vehicles.Where(v => v.ClientId == id_client).ToListAsync();
            } else
            {
                vehicles = await _context.Vehicles.Where(v => v.ClientId == id_client).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            var response = new ApiResponse<object>
            {
                Data = vehicles,
                Message = "Todos los vehiculos",
                Status = "success",
                Length = vehiclesLength
            };
            return Ok(response);
        }

        [HttpGet]
        // [Authorize]
        [Route("{id_client}/by_id/{id_vehicle}")]
        public async Task<IActionResult> GetVehicle(int id_client, int id_vehicle)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id_vehicle && v.ClientId == id_client);
            if (vehicle is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Data = new {},
                    Message = "Vehiculo no encontrado",
                    Status = "error"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Data = vehicle,
                Message = "Vehiculo por id",
                Status = "success"
            });
        }

        [HttpPost]
        // [Authorize]
        [Route("")]
        public async Task<IActionResult> Createvehicle(VehicleDto vehicle)
        {
            try
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

                return Ok(new ApiResponse<object>
                {
                    Data = vehicle,
                    Message = "Vehiculo creado",
                    Status = "success"
                });

            } catch (Exception)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Data = new {},
                    Message = "Error al crear el vehiculo",
                    Status = "error"
                });
            }
        }

        [HttpPut]
        // [Authorize]
        [Route("{id_client}/{id_vehicle}")]
        public async Task<IActionResult> UpdateVehicle(int id_client, int id_vehicle, VehicleDto vehicle)
        {
            try
            {
                var vehicleCurrent = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id_vehicle && v.ClientId == id_client);
                if (vehicleCurrent is null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = new { },
                        Message = "Vehiculo no encontrado",
                        Status = "error"
                    });
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
                return Ok(new ApiResponse<object>
                {
                    Data = vehicle,
                    Message = "Vehiculo por id",
                    Status = "success"
                });

            } catch (Exception)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Data = new { },
                    Message = "Error al actualizar el vehiculo",
                    Status = "error"
                });
            }
        }

        [HttpDelete]
        // [Authorize]
        [Route("{id_client}/{id_vehicle}")]
        public async Task<IActionResult> DeleteVehicle(int id_client, int id_vehicle)
        {
            try
            {
                var vehicleRemove = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id_vehicle && v.ClientId == id_client);
                if (vehicleRemove is null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = new { },
                        Message = "Vehiculo no encontrado",
                        Status = "error"
                    });
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
                return Ok(new ApiResponse<object>
                {
                    Data = new { },
                    Message = "Vehiculo eliminado",
                    Status = "success"
                });

            } catch (Exception)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Data = new { },
                    Message = "Error al eliminar el vehiculo",
                    Status = "error"
                });
            }
        }
    }
}
