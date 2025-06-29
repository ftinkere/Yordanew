using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

public class LanguageDbo {
    public Guid Id { get; set; }
    [ForeignKey(nameof(Author))]
    public Guid AuthorId { get; set; }
    public Guid[] EditorsIds { get; set; } = [];
    public required string Name { get; set; }
    public string? AutoName { get; set; } = null;
    public string? AutoNameTranscription { get; set; } = null;
    public string? Description { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    
    public virtual AppUser Author { get; set; }
    public virtual ICollection<ArticleDbo> Articles { get; set; } = [];
}