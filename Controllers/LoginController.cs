using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using apiNew.Models;
using apiNew.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
namespace apiNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private readonly ApplicationDbContext _context;
        public LoginController(IConfiguration configuration, AuthService authService, ApplicationDbContext context)
        {
            _configuration = configuration;
            _authService = authService;
            _context = context;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            // Lógica de autenticación aquí
            Console.WriteLine(login);
            if (_authService.AuthenticateUser(login))
            {
                if (login.Username != null)
                {
                    var token = GenerateJwtToken(login.Username);
                    return Ok(new { Token = token });
                }
            }
            return Unauthorized();

        }

        [HttpPost("register")]
        public bool RegisterUser(Users user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash) || string.IsNullOrWhiteSpace(user.Username))
            {
                return false; // Manejar casos donde el registro es nulo o las credenciales están en blanco
            }

            // Verificar si el nombre de usuario ya está en uso
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return false; // Nombre de usuario ya en uso, manejar según sea necesario
            }

            // Generar un nuevo salt único para este usuario
            var salt = GenerateSalt();

            // Hashear la contraseña con el salt
            var hashedPassword = HashPassword(user.PasswordHash, salt);

            // Actualizar el objeto usuario con la información de salt y contraseña hasheada
            user.Salt = salt;
            user.PasswordHash = hashedPassword;

            // Agregar el nuevo usuario a la base de datos
            _context.Users.Add(user);
            _context.SaveChanges();

            return true; // Registro exitoso
        }

        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Combina la contraseña y la sal antes de hashear
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

                // Convierte el hash a una representación hexadecimal
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? string.Empty); // Configuración de la clave secreta

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, username),
            // Agregar más claims según sea necesario
        }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["Jwt:ExpireHours"])), // Configuración del tiempo de expiración
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

       

        private string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
//https://chat.openai.com/share/e7490318-0d7d-4d2b-bfb0-eadabe569315