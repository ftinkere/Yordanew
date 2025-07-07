using Yordanew.Models;
using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class Lexeme {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public Guid ArticleId { get; init; } = Guid.Empty;
    public IList<int> Path { get; set; } = [1];
    public IList<string> Tags { get; set; } = [];
    
    public RichText Description { get; set; } = new RichText(string.Empty);
    
    public override bool Equals(object? obj) {
        if (obj is Lexeme pos) {
            return Id.Equals(pos.Id);
        }
        return false;
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}

public static class LexemeExtensions {
    public static Lexeme ToDomain(this LexemeDbo dbo) {
        return new Lexeme {
            Id = dbo.Id,
            ArticleId = dbo.ArticleId,
            Description = new RichText(dbo.Description ?? string.Empty),
            Path =  dbo.Path.ToList(),
            Tags = dbo.Tags.ToList()
        };
    }
}