using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Astrolabe.Data;
using Astrolabe.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Astrolabe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // DTO для приёма логина и пароля с фронтенда
    public record AuthRequest(string Username, string Password);

    // 1. РЕГИСТРАЦИЯ: POST /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        var username = request.Username.Trim();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Введите логин и пароль" });

        // Проверяем, не занято ли имя
        var exists = await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower());
        if (exists)
            return BadRequest(new { message = "Это имя уже занято" });

        var user = new User
        {
            Username = username,
            PasswordHash = HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Сразу выдаём пользователю Cookie входа
        await SignInUser(user);

        return Ok(new { user.Id, user.Username });
    }

    // 2. ВХОД: POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var username = request.Username.Trim();
        var hash = HashPassword(request.Password);

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower() && u.PasswordHash == hash);

        if (user == null)
            return Unauthorized(new { message = "Неверный логин или пароль" });

        await SignInUser(user);

        return Ok(new { user.Id, user.Username });
    }

    // 3. ПРОВЕРКА ТЕКУЩЕГО ПОЛЬЗОВАТЕЛЯ: GET /api/auth/me
    [HttpGet("me")]
    public IActionResult Me()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.Identity.Name;

        return Ok(new { id, username });
    }

    // 4. ВЫХОД: POST /api/auth/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    // Вспомогательный метод: шифруем Id и Username в Cookie браузера
    private async Task SignInUser(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    // Вспомогательный метод: превращаем пароль в SHA256-хеш
    private static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }
}