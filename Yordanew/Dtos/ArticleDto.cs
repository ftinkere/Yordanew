using Yordanew.Domain.Entity;

namespace Yordanew.Dtos;

public class ArticleDto {
    public Guid Id { get; set; }
    public Guid LanguageId { get; set; }
    public required string Lemma { get; set; }
    public string? Transcription { get; set; }
    public string? Adaptation { get; set; }
    
    public virtual ICollection<LexemeDto> Lexemes { get; set; } = [];
}

public static class ArticleDtoMapper {
    public static ArticleDto ToDto(this Article article) {
        return new ArticleDto {
            Id = article.Id,
            LanguageId = article.LanguageId,
            Lemma = article.Lemma.Content,
            Transcription = article.Lemma.Transcription,
            Adaptation = article.Lemma.Adaptation,
            Lexemes = article.Lexemes.Select(l => l.ToDto()).ToList(),
        };
    }
}