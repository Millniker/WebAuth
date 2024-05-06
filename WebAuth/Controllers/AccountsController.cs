using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pathnostics.Web.Models.DTO;
using Pathnostics.Web.Models.Entity;
using Pathnostics.Web.Services;
using Pathnostics.Web.Services.Interface;
using university.Server.Exception.ExceptionsModels;
using WebAuth.Data;
using WebAuth.Services;

namespace WebAuth.Controllers;

[ApiController]
[Route("accounts")]
[EnableCors]
public class AccountsController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly ITokenService _tokenService;
    private readonly HashPasswords _hash;
    private readonly ThrowException _exception;

    public AccountsController(ITokenService tokenService, ApplicationContext context, HashPasswords hash, ThrowException exception)
    {
        _hash = hash;
        _tokenService = tokenService;
        _context = context;
        _exception = exception;

    }

    [HttpPost("login")]
    public async Task<ActionResult<Response>> Authenticate(UserDto user)
    {
        if (!_exception.GenerateRandomValue())   return StatusCode(500);
        if (!HttpContext.Request.Headers.TryGetValue("RequestId", out var requestId))
        {
            throw new IncorrectDataException("не передан requestId");
        }

        if (!HttpContext.Request.Headers.TryGetValue("DeviceId", out var deviceId))
        {
            throw new IncorrectDataException("не передан deviceId");
        }
        if (!ModelState.IsValid)
        {
            throw new IncorrectDataException("Неккоректный email");
        }
        
        var checkUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (checkUser is null)
            throw new ItemNotFoundException($"Пользователь с почтой {user.Email} не найден");
        if (_hash.HashPassword(user.Password) != _hash.HashPassword(checkUser.Password))
        {
            throw new IncorrectDataException("Неккоректный пароль");
        }
        var accessToken = _tokenService.GenerateToken(checkUser.Id, user.Email);
        
        await _context.SaveChangesAsync();
        
        return Ok(new Response
        {
            Token = accessToken
        });
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<Response>> Register(RegisterRequest request)
    {
        if (!_exception.GenerateRandomValue())   return StatusCode(500);
        if (!HttpContext.Request.Headers.TryGetValue("RequestId", out var requestId))
        {
            throw new IncorrectDataException("не передан requestId");
        }

        if (!HttpContext.Request.Headers.TryGetValue("DeviceId", out var deviceId))
        {
            throw new IncorrectDataException("не передан deviceId");
        }
        var findUser = await _context.User.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (findUser != null)
        {
            throw new IncorrectDataException($"Почта {request.Email} уже занята");
        }

        var userId = Guid.NewGuid(); 
        await _context.User.AddAsync(new UserEntity
        {
            Id = userId,
            Email = request.Email,
            Password = request.Password
        });
        await _context.SaveChangesAsync();
        var accessToken = _tokenService.GenerateToken(userId, request.Email);

        return Ok(new Response
        {
            Token = accessToken
        });
    }

    [HttpGet("email/{token}")]
    public  ActionResult<EmailDto> Email(string token)
    {
        if (!_exception.GenerateRandomValue())   return StatusCode(500);
        if (!HttpContext.Request.Headers.TryGetValue("RequestId", out var requestId))
        {
            throw new IncorrectDataException("не передан requestId");
        }

        if (!HttpContext.Request.Headers.TryGetValue("DeviceId", out var deviceId))
        {
            throw new IncorrectDataException("не передан deviceId");
        }
        var payload = _tokenService.ParseToken(token);
        return Ok(new EmailDto
        {
            Email = payload["email"].ToString()
        });
    }
}