using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

[Table("Files")]
public class FileDbo {
    public Guid Id { get; set; }
    public Guid UserId { get; set; } = Guid.Empty;
    public required string FileName { get; set; }
    public required string MimeType { get; set; }
    public long Size { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public virtual ICollection<FileRelationDbo> Relations { get; set; } = [];
}
