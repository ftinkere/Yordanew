namespace Yordanew.Models;

public class ArticleDbo {
    public Guid Id { get; set; }
    public Guid LanguageId { get; set; }
    public required string Lemma { get; set; }
    public string? Transcription { get; set; }
    public string? Adaptation { get; set; }
    
    public virtual ICollection<LexemeDbo> Lexemes { get; set; } = [];
}