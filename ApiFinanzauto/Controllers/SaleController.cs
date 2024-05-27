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
    public class SaleController : ControllerBase
    {
        private readonly FinanzautoDbContext _context;

        public SaleController(FinanzautoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Createvehicle(SaleDto sale)
        {
            try
            {
                Sale saleNew = new Sale();
                saleNew.Name = sale.Name;
                saleNew.Document = sale.Document;
                saleNew.Phone = sale.Phone;
                saleNew.Email = sale.Email;
                saleNew.VehicleId = sale.VehicleId;
                saleNew.Status = sale.Status;
                saleNew.CreatedAt = DateTime.Now;
                saleNew.UpdatedAt = DateTime.Now;

                await _context.Sales.AddAsync(saleNew);

                var vehicleCurrent = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == sale.VehicleId);
                vehicleCurrent!.SalesStatus = "Vendido";

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Data = sale,
                    Message = "Vehiculo comprado",
                    Status = "success"
                });

            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Data = new { },
                    Message = "Error al comprar el vehiculo",
                    Status = "error"
                });
            }
        }
    }
}
