using System;

namespace AuthenticationService.Infrastructure.Persistence.Repositories.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(string id);
    Task<IEnumerable<Company>> GetAllAsync();
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task DeleteAsync(string id);
}
