using System;

namespace AuthenticationService.Infrastructure.Persistence.Repositories.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task AddAsync(ApplicationUser user);
    Task UpdateAsync(ApplicationUser user);
    Task DeleteAsync(string id);
}