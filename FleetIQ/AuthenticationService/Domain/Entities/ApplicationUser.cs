using System;
using AuthenticationService.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CompanyId { get; set; }
    public Address Address { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }

    public virtual Company Company { get; set; }
}
