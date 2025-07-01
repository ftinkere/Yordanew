using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

[Table("Articles")]
public class ArticleDbo {
    public Guid Id { get; set; }
    [ForeignKey(nameof(Language))]
    public Guid LanguageId { get; set; }
    public required string Lemma { get; set; }
    public string? Transcription { get; set; }
    public string? Adaptation { get; set; }
    
    public virtual LanguageDbo Language { get; set; }
    public virtual ICollection<LexemeDbo> Lexemes { get; set; } = [];
}