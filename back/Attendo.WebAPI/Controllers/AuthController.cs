using System.Security.Claims;
using Attendo.Application.DTOs.Auth;
using Attendo.Domain.Entities;
using Attendo.Infrastructure.Auth;
using Attendo.Infrastructure.Security;
using Attendo.Infrastructure.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendo.WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenGenerator _jwt;

    public AuthController(IUserRepository users, IPasswordHasher hasher, IJwtTokenGenerator jwt)
    {
        _users = users;
        _hasher = hasher;
        _jwt = jwt;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req, CancellationToken ct)
    {
        if (await _users.ExistsLoginAsync(req.Login, ct))
            return Conflict(new { field = "login", message = "taken" });

        if (await _users.ExistsEmailAsync(req.Email, ct))
            return Conflict(new { field = "email", message = "taken" });

        var user = new User
        {
            Login = req.Login.Trim(),
            Email = req.Email.Trim(),
            Role = string.IsNullOrWhiteSpace(req.Role) ? "User" : req.Role.Trim(),
            PasswordHash = _hasher.Hash(req.Password)
        };

        await _users.AddAsync(user, ct);
        return Ok(new { user.Id, user.Login, user.Email, user.Role });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req, CancellationToken ct)
    {
        var user = await _users.FindByLoginOrEmailAsync(req.Login.Trim(), ct);
        if (user is null) return Unauthorized(new { message = "Invalid credentials" });

        if (!_hasher.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { message = "Invalid credentials" });

        var (token, exp) = _jwt.CreateToken(user);
        return Ok(new AuthResponse { AccessToken = token, ExpiresAt = exp });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<AuthUserResponse>> Me(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _users.FindByLoginOrEmailAsync(User.Identity!.Name!, ct);
        if (user is null) return Unauthorized();

        return Ok(new AuthUserResponse
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            Role = user.Role
        });
    }
}
