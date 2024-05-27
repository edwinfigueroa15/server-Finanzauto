using ApiFinanzauto.Dtos;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [Route("")]
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

                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "public");
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
                    Data = date + file.FileName,
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
        [Authorize]
        [Route("{path}")]
        public IActionResult DeleteFile(string path)
        {
            var pathFile = Path.Combine(_webHostEnvironment.ContentRootPath, "public/"+path);
            if (System.IO.File.Exists(pathFile))
            {
                System.IO.File.Delete(pathFile);
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
