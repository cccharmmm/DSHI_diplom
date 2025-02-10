using Microsoft.AspNetCore.Mvc;
using DSHI_diplom.Services.Interfaces;
using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly DiplomContext _diplomContext;

    public AuthController(IUserService userService, ITokenService tokenService, DiplomContext diplomContext)
    {
        _userService = userService;
        _tokenService = tokenService;
        _diplomContext = diplomContext;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] DSHI_diplom.Model.LoginRequest request)

    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Логин и пароль обязательны");
        }
        var login = request.Login.Trim();
        var password = request.Password.Trim();
        var user = await _diplomContext.Users
        .FirstOrDefaultAsync(u => u.Login == request.Login && u.Password == request.Password);

        if (user == null)
        {
            return Unauthorized("Неверный логин или пароль");
        }

        //if (user.Role == null || user.Role.Name.ToLower() != "user")
        //{
        //    return Forbid();
        //}

        var token = _tokenService.GenerateToken(user);
        return Ok(new { token });
    }
}
