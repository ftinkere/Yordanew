using Yordanew.Domain.Entity;

namespace Yordanew.Dtos;

public class LexemeDto {
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string? Description { get; set; }
    public int[] Path { get; set; } = [];
}

public static class LexemeDtoMapper {
    public static LexemeDto ToDto(this Lexeme lexeme) {
        return new LexemeDto {
            Id = lexeme.Id,
            ArticleId = lexeme.ArticleId,
            Description = lexeme.Description.Content,
            Path = lexeme.Path.ToArray(),
        };
    }
}