using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class LexemeGroup : ILexemeItem {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    
    public IEnumerable<ILexemeItem> Children { get; set; } = new List<ILexemeItem>();
    
    public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
    public GramList? GramList { get; set; } = null;
}