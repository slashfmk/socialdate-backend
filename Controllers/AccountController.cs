using System.Security.Cryptography;
using System.Text;
using socialbackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using socialbackend.Dtos;
using socialbackend.Entities;

namespace socialbackend.Controllers;

public class AccountController : BaseApiController
{

    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<ActionResult<AppUser>> Register(RegisterUserDto registerUser)
    {

        if (await this.UserExists(registerUser.UserName.ToLower())) return BadRequest(new {message = "User already exists"});
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            // Id = 55,
            UserName = registerUser.UserName.ToLower(), 
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password)),
            PasswordSalt = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUser.Password))
        };

        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return user;
    }
    
    [HttpGet]
    public async Task<ActionResult<AppUser>> GetUsers()
    {
        var result = await _context.Users.ToListAsync();
        return Ok(result);
    }

    private async Task<bool> UserExists(string username)
    {
        return await  _context.Users.AnyAsync(x => x.UserName == username.ToLower());;
    }

}