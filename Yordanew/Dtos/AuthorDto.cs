
using Yordanew.Domain.Entity;

namespace Yordanew.Dtos;

public class AuthorDto {
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public string? DisplayName { get; set; }
}

public static class AuthorDtoMapper {
    public static AuthorDto ToDto(this Author author) {
        return new AuthorDto {
            Id = author.Id,
            UserName = author.UserName,
            DisplayName = author.DisplayName,
        };
    }
}