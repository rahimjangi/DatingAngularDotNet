using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class AppUsersController : BaseApiController
{
    private readonly IAppUserRepository _context;
    private readonly IMapper _mapper;

    public AppUsersController(IAppUserRepository context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    //[AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var usersFromDb = await _context.GetAppUsersAsync();
        var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(usersFromDb);
        return Ok(usersToReturn);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MemberDto>> GetUser(int id)
    {
        var userFromDb = await _context.GetAppUserByIdAsync(id);
        var userToReturn = _mapper.Map<MemberDto>(userFromDb);
        if (userFromDb != null)
        {
            return userToReturn;
        }
        return NotFound();
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUserByName(string username)
    {
        var userFromDb = await _context.GetAppUserByNameAsync(username);
        var userToReturn = _mapper.Map<MemberDto>(userFromDb);
        if (userFromDb != null)
        {
            return userToReturn;
        }
        return NotFound();
    }
}
