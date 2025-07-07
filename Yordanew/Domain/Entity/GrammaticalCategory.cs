using Yordanew.Domain.ValueObjects;
using Yordanew.Models;

namespace Yordanew.Domain.Entity;

public class GrammaticalCategory {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid PosId { get; set; }
    public required Name Name { get; set; }
    public Code Code { get; set; } = new Code(string.Empty);
    public RichText Description { get; set; } = new RichText("");
    
    public IEnumerable<GrammaticalFeature> GrammaticValues { get; set; } = new List<GrammaticalFeature>();
    
    
    public void AddGrammaticValue(GrammaticalFeature value) {
        GrammaticValues = GrammaticValues.Append(value);
    }
    
    public override bool Equals(object? obj) {
        if (obj is GrammaticalCategory pos) {
            return Id.Equals(pos.Id);
        }
        return false;
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}

public static class CategoryExtensions {
    public static GrammaticalCategory ToDomain(this GrammaticalCategoryDbo dbo) {
        return new GrammaticalCategory {
            Id = dbo.Id,
            Name = new Name(dbo.Name),
            Code = new Code(dbo.Code),
            Description = new RichText(dbo.Description),
            PosId = dbo.PartOfSpeechId,
        };
    }
}