using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

public class LexemeDbo {
    public Guid Id { get; set; }
    
    [ForeignKey(nameof(Article))]
    public Guid ArticleId { get; set; }
    public string? Description { get; set; }
    public int[] Path { get; set; } = [];

    public virtual ArticleDbo Article { get; set; }
}