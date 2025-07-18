using Yordanew.Domain.ValueObjects;
using Yordanew.Models;

namespace Yordanew.Domain.Entity;

public class PartOfSpeech {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid LanguageId { get; set; }
    public required Name Name { get; set; }
    public required Code Code { get; set; } = new Code(string.Empty);
    public RichText Description { get; set; } = new RichText("");

    public override bool Equals(object? obj) {
        if (obj is PartOfSpeech pos) {
            return Id.Equals(pos.Id);
        }
        return false;
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}

public static class PosExtensions {
    public static PartOfSpeech ToDomain(this PartOfSpeechDbo dbo) {
        return new PartOfSpeech {
            Id = dbo.Id,
            Name = new Name(dbo.Name),
            Code = new Code(dbo.Code),
            Description = new RichText(dbo.Description),
            LanguageId = dbo.LanguageId ?? Guid.Empty,
        };
    }
}