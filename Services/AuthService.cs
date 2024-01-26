using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using apiNew.Models;

namespace apiNew.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool AuthenticateUser(Login login)
    {
        if (login == null || string.IsNullOrWhiteSpace(login.Password) || string.IsNullOrWhiteSpace(login.Username))
        {
            return false; // Manejar casos donde el login es nulo o las credenciales están en blanco
        }

        // Buscar el usuario por nombre de usuario
        var user = _context.Users.FirstOrDefault(u => u.Username == login.Username);

        if (user != null)
        {
            // Verificar la contraseña
            var authResult = VerifyPassword(login.Password, user.PasswordHash, user.Salt);

            if (authResult.IsSuccessful)
            {
                // Contraseña válida
                Console.WriteLine(authResult.Message);
                return true;
            }
            else
            {
                // Contraseña incorrecta
                Console.WriteLine(authResult.Message);
                return false;
            }
        }

        return false;
    }

    public AuthResult VerifyPassword(string inputPassword, string? hashedPassword, string? salt)
    {
        if (hashedPassword == null || salt == null)
        {
            // Devuelve un resultado de autenticación que indica un problema (por ejemplo, contraseña no establecida)
            return new AuthResult { IsSuccessful = false, Message = "Contraseña no establecida" };
        }

        using (SHA256 sha256 = SHA256.Create())
        {
            // Aplica el salt al inputPassword antes de calcular el hash
            var saltedPassword = $"{inputPassword}{salt}";
            var inputPasswordHash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword)))
                .Replace("-", string.Empty);

            if (string.Equals(inputPasswordHash, hashedPassword, StringComparison.OrdinalIgnoreCase))
            {
                return new AuthResult { IsSuccessful = true, Message = "Contraseña válida" };
            }
            else
            {
                return new AuthResult { IsSuccessful = false, Message = "Contraseña incorrecta" };
            }
        }
    }

    public class AuthResult
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
    }
}