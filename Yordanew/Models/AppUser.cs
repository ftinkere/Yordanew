using Microsoft.AspNetCore.Identity;

namespace Yordanew.Models;

public class AppUser : IdentityUser<Guid> {
    public string? DisplayName { get; set; }
}