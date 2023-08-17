﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class AppUsersController : BaseApiController
{
    private readonly AppDbContext _context;

    public AppUsersController(AppDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        var result = await _context.AppUsers.ToListAsync();
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var userFromDb = await _context.AppUsers
            .FirstOrDefaultAsync(x => x.Id == id);
        if (userFromDb != null)
        {
            return userFromDb;
        }
        return NotFound();
    }
}
