using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly AppDbContext _context;

    public BuggyController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize]

    [HttpGet("auth")]
    public ActionResult<string> GetSecret()
    {
        return "Secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = _context.AppUsers.Find(-1);
        if (thing == null) return NotFound("Not FOUND");

        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        var thing = _context.AppUsers.Find(-1);
        //if (thing == null) return NotFound("Not FOUND");

        return thing.ToString();
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        var thing = _context.AppUsers.Find(-1);
        //if (thing == null) return NotFound("Not FOUND");

        return BadRequest("Bad Request");
    }


}
