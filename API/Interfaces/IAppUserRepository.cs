using API.Entities;

namespace API.Interfaces;

public interface IAppUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetAppUsersAsync();
    Task<AppUser> GetAppUserByIdAsync(int id);
    Task<AppUser> GetAppUserByNameAsync(string name);
}
