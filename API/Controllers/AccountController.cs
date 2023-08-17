using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
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

        var userDto = new UserDto()
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
        };

        return userDto;


    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.AppUsers.FirstOrDefaultAsync(x => x.UserName == loginDto.Username);
        if (user == null) return Unauthorized(loginDto.Username);

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("User or password is not valid!");
        }

        var userDto = new UserDto()
        {
            Username = loginDto.Username,
            Token = _tokenService.CreateToken(user),
        };

        return userDto;
    }
    private async Task<bool> UserExists(string username)
    {
        var userDb = await _context.AppUsers.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        return userDb;
    }
}
