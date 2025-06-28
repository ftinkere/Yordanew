
namespace Yordanew.Models;

public class LanguageDbo {
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public Guid[] EditorsIds { get; set; } = [];
    public required string Name { get; set; }
    public string? AutoName { get; set; } = null;
    public string? AutoNameTranscription { get; set; } = null;
    public string? Description { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<ArticleDbo> Articles { get; set; } = [];
}