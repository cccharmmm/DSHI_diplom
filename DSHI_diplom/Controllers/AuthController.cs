using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;
using DSHI_diplom.Services.Implementations;
using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly DiplomContext _diplomContext;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(IUserService userService, DiplomContext diplomContext, ILogger<AuthController> logger, IConfiguration configuration)
    {
        _userService = userService;
        _diplomContext = diplomContext;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
        {
            _logger.LogWarning("Некорректные данные для входа.");
            return BadRequest("Логин и пароль обязательны.");
        }

        _logger.LogInformation("Попытка входа: Login = {Login}, Password = {Password}", request.Login, request.Password);

        var user = await _userService.AuthenticateAsync(request.Login, request.Password);
        if (user == null)
        {
            _logger.LogWarning("Не найден пользователь с Login = {Login}", request.Login);
            return Unauthorized();
        }

        _logger.LogInformation("Найден пользователь: {Login}", user.Login);
        if (user.Role?.Name != "user")
        {
            _logger.LogWarning("Пользователь {Login} не имеет роли 'user'.", user.Login);
            return Forbid();
        }

        _logger.LogInformation("Пароль верный, генерируем токен...");
        var token = GenerateJwtToken(user);

        var response = new LoginResponse
        {
            Token = token,
            UserId = user.Id,
            Login = user.Login,
            Role = user.Role?.Name ?? "user"
        };

        _logger.LogInformation("Успешный вход: {Login}, Token = {Token}", user.Login, token);
        return Ok(response);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "user")
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        _logger.LogInformation("Ключ JWT: {Key}", _configuration["Jwt:Key"]);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));
        _logger.LogInformation("Токен истекает в {Expires}", expires);


        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

   
}