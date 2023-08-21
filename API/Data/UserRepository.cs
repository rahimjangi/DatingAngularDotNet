using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IAppUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<AppUser> GetAppUserByIdAsync(int id)
    {
        return await _context.AppUsers
            .Include(x => x.Photos)
            .SingleOrDefaultAsync(p => p.Id == id);
    }

    public async Task<AppUser> GetAppUserByNameAsync(string name)
    {
        var userFromDb = await _context.AppUsers
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.UserName.ToLower() == name.ToLower());
        if (userFromDb == null) return null;
        return userFromDb;
    }

    public async Task<IEnumerable<AppUser>> GetAppUsersAsync()
    {
        return await _context.AppUsers
            .Include(x => x.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}
