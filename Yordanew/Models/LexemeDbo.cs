namespace Yordanew.Models;

public class LexemeDbo {
    public Guid Id { get; set; }
    public Guid ArticleId { get; set; }
    public string? Description { get; set; }
    public int[] Path { get; set; } = [];

    public virtual ArticleDbo Article { get; set; }
}