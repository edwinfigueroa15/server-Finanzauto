using ApiFinanzauto.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ApiFinanzauto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        // [Authorize]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            var date = DateTime.Now.GetHashCode();
            try
            {
                if (file.Length == 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Data = new { },
                        Message = "Archivo no encontrado",
                        Status = "error"
                    });
                }

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

                return Ok(new ApiResponse<object>
                {
                    Data = fullPath,
                    Message = "Archivo guardado",
                    Status = "success"
                });
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Data = new { },
                    Message = "Archivo al guardar el archivo",
                    Status = "error"
                });
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
                return Ok(new ApiResponse<object>
                {
                    Data = new {},
                    Message = "Archivo eliminado",
                    Status = "success"
                });
            }

            return NotFound(new ApiResponse<object>
            {
                Data = new { },
                Message = "El archivo no existe",
                Status = "error"
            });
        }
    }
}
