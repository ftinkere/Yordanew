using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class GrammaticCategory(Transcriptable name) {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Transcriptable Name { get; set; } = name;
    public RichText Description { get; set; } = new RichText("");
    
    public IEnumerable<GrammaticValue> GrammaticValues { get; set; } = new List<GrammaticValue>();
    
    
    public void AddGrammaticValue(GrammaticValue value) {
        GrammaticValues = GrammaticValues.Append(value);
    }
}