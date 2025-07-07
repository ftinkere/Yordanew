using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

[Table("Lexemes")]
public class LexemeDbo {
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Article))]
    public Guid ArticleId { get; set; }
    public string? Description { get; set; }
    public int[] Path { get; set; } = [];
    public string[] Tags { get; set; } = [];

    public virtual ArticleDbo Article { get; set; }
}