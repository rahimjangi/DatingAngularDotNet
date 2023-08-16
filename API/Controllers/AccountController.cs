using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("User already exist");


        using var hmac = new HMACSHA512();
        var user = new AppUser()
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };
        _context.AppUsers.Add(user);
        await _context.SaveChangesAsync();

        return registerDto;


    }


    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
        if (user == null) return Unauthorized(loginDto.Username);

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("User or password is not valid!");
        }


        return user;
    }
    private async Task<bool> UserExists(string username)
    {
        var userDb = await _context.AppUsers.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        return userDb;
    }
}
