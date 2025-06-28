using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class PartOfSpeech(Transcriptable name) {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Transcriptable Name { get; set; } = name;
    public RichText Description { get; set; } = new RichText("");
    
    public IEnumerable<GrammaticCategory> GrammaticCategories { get; set; } = new List<GrammaticCategory>();

    public void AddGrammaticCategory(GrammaticCategory category) {
        GrammaticCategories = GrammaticCategories.Append(category);
    }
}