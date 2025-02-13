using System;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Models;

public class ApplicationUser : IdentityUser
{
    public required string FullName { get; set; }
}
