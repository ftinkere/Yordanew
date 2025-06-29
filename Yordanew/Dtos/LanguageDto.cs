
using Yordanew.Domain.Entity;
using Yordanew.Models;

namespace Yordanew.Dtos;

public class LanguageDto {
    public Guid Id { get; set; }
    public required AuthorDto Author { get; set; }
    public required string Name { get; set; }
    public string? AutoName { get; set; } = null;
    public string? AutoNameTranscription { get; set; } = null;
    public string? Description { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<ArticleDto> Articles { get; set; } = [];
}

public static class LanguageDtoMapper {
    public static LanguageDto ToDto(this Language language) {
        return new LanguageDto {
            Id = language.Id,
            Author = language.Author.ToDto(),
            Name = language.Name.Content,
            AutoName = language.Name.Translation,
            AutoNameTranscription = language.Name.Transcription,
            Description = language.Description.Content,
            IsPublished = language.IsPublished,
            CreatedAt = language.CreatedAt,
            Articles = language.Articles.Select(a => a.ToDto()).ToList(),
        };
    }
}