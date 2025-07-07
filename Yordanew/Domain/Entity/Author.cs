using Yordanew.Models;

namespace Yordanew.Domain.Entity;

public class Author {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string UserName { get; set; }
    public string? DisplayName { get; set; }
    
    public override bool Equals(object? obj) {
        if (obj is Author pos) {
            return Id.Equals(pos.Id);
        }
        return false;
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
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