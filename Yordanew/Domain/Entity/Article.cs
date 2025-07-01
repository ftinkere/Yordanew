using Yordanew.Models;
using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class Article(Transcriptable lemma) {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid LanguageId { get; set; } = Guid.Empty;
    public Transcriptable Lemma { get; set; } = lemma;
    
    public IEnumerable<Lexeme> Lexemes { get; set; } = new List<Lexeme>();
    public IEnumerable<Guid> Files { get; set; } = new List<Guid>();

    public void AddLexeme(Lexeme lexeme) {
        Lexemes = Lexemes.Append(lexeme);
    }
}

public static class ArticleExtensions {
    public static Article ToDomain(this ArticleDbo dbo) {
        return new Article(new Transcriptable(dbo.Lemma ?? string.Empty, dbo.Transcription, dbo.Adaptation)) {
            Id = dbo.Id,
            LanguageId = dbo.LanguageId,
            Lexemes = dbo.Lexemes.Select(l => l.ToDomain()).ToList(),
            Files = dbo.Files,
        };
    }
}