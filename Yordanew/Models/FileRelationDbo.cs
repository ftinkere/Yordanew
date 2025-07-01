using System.ComponentModel.DataAnnotations.Schema;

namespace Yordanew.Models;

[Table("FileRelations")]
public class FileRelationDbo {
    public Guid Id { get; set; }

    [ForeignKey(nameof(File))]
    public Guid FileId { get; set; }
    public Guid EntityId { get; set; }
    public required string EntityName { get; set; }

    public virtual FileDbo File { get; set; } = null!;
}
