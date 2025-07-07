using Yordanew.Domain.ValueObjects;
using Yordanew.Models;

namespace Yordanew.Domain.Entity;

public class GrammaticalFeature {
    public Guid Id { get; init;  } = Guid.CreateVersion7();
    public Guid CategoryId { get; set; }
    public required Name Name { get; set; }
    public Code Code { get; set; } = new Code(string.Empty);
    public RichText Description { get; set; } = new RichText("");
    
    public override bool Equals(object? obj) {
        if (obj is GrammaticalFeature pos) {
            return Id.Equals(pos.Id);
        }
        return false;
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}

public static class FeatureExtensions {
    public static GrammaticalFeature ToDomain(this GrammaticalFeatureDbo dbo) {
        return new GrammaticalFeature() {
            Id = dbo.Id,
            Name = new Name(dbo.Name),
            Code = new Code(dbo.Code),
            Description = new RichText(dbo.Description),
            CategoryId = dbo.CategoryId,
        };
    }
}