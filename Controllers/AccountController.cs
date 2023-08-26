using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using socialbackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialbackend.Dtos;
using socialbackend.Entities;
using socialbackend.interfaces;

namespace socialbackend.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;    
    }

    
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterUserDto registerUser)
    {
        if (await UserExists(registerUser.UserName.ToLower()))
            return BadRequest(new { message = "User already exists" });
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerUser.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password)),
            PasswordSalt = hmac.Key
        };

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<AppUser>> GetUsers()
    {
        var result = await _context.Users.ToListAsync();
        return Ok(result);
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        ;
    }

    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> LogIn(LoginDto loginDto)
    {
        var foundUser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

        if (foundUser is null) return Unauthorized();

        using var hmac = new HMACSHA512(foundUser.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (var i = 0; i < computedHash.Length; i++)
            if (computedHash[i] != foundUser.PasswordHash[i])
                return Unauthorized(new { error = "Please check your username or password" });

        
        return new UserDto
        {
            Username = foundUser.UserName,
            Token = _tokenService.CreateToken(foundUser)
        };
    }
}