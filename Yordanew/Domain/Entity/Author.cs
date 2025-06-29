using Yordanew.Models;

namespace Yordanew.Domain.Entity;

public class Author {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string UserName { get; set; }
    public string? DisplayName { get; set; }
}

public static class AuthorExtensions {
    public static Author ToDomain(this AppUser user) {
        return new Author {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            DisplayName = user.DisplayName,
        };
    }
}