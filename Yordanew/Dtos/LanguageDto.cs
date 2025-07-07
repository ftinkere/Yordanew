using Yordanew.Domain.Entity;

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
    
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; } = 1;

    public ICollection<ArticleDto> Articles { get; set; } = [];

    public IList<PartOfSpeechDto> PartsOfSpeech { get; set; } = new List<PartOfSpeechDto>();
}

public static class LanguageDtoMapper {
    public static LanguageDto ToDto(this Language language, int page = 1, int pageSize = 10, int totalCount = 0) {
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
            PartsOfSpeech = language.Grammatic.Select(kvp => kvp.Key.ToDto(kvp.Value)).ToList(),
            
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}