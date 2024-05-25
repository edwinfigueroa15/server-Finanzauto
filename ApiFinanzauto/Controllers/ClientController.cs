using ApiFinanzauto.Dtos;
using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiFinanzauto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly FinanzautoDbContext _context;
        private readonly IConfiguration _config;

        public ClientController(FinanzautoDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string HashPassword(string pass)
        {
            return BCrypt.Net.BCrypt.HashPassword(pass); ;
        }

        private bool VerifyPassword(string pass, string passHash)
        {
            return BCrypt.Net.BCrypt.Verify(pass, passHash);
        }

        private string GenerateJwt(Client client)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", client.Id.ToString()),
                new Claim("email", client.Email.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(LoginDto login)
        {
            var currentClient = await _context.Clients.FirstOrDefaultAsync(client => client.Email == login.Email);
            if (currentClient is not null)
            {
                if (VerifyPassword(login.Password, currentClient.Password))
                {
                    var token = GenerateJwt(currentClient);
                    return Ok(token);
                }

                return NotFound("Los datos no coinciden");
            }

            return NotFound("Cliente no encontrado");
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> CreateUser(ClientDto clientBody)
        {
            var clinetNew = new Client();
            clinetNew.Name = clientBody.Name;
            clinetNew.Email = clientBody.Email;
            clinetNew.Password = HashPassword(clientBody.Password);
            clinetNew.CreatedAt = DateTime.Now;
            clinetNew.UpdatedAt = DateTime.Now;

            var userNew = await _context.Clients.AddAsync(clinetNew);
            if (userNew is not null)
            {
                await _context.SaveChangesAsync();
                var currentClient = await _context.Clients.FirstOrDefaultAsync(client => client.Email == clientBody.Email);

                if(currentClient is not null)
                {
                    var token = GenerateJwt(currentClient);
                    return Ok(token);
                }

                return NotFound("Error al crear el token");

            }

            return NotFound("Error al crear el cliente");
        }
    }
}
