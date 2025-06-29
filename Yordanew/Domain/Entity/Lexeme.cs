using Yordanew.Models;
using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class Lexeme : ILexemeItem {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid ArticleId { get; set; } = Guid.Empty;
    public IList<int> Path { get; set; } = [1];
    
    public RichText Description { get; set; } = new RichText(string.Empty);
}

public static class LexemeExtensions {
    public static Lexeme ToDomain(this LexemeDbo dbo) {
        return new Lexeme {
            Id = dbo.Id,
            ArticleId = dbo.ArticleId,
            Description = new RichText(dbo.Description ?? string.Empty),
            Path =  dbo.Path?.ToList() ?? [1],
        };
    }
}