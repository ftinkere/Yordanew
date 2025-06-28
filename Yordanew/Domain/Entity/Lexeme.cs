using Yordanew.Models;
using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class Lexeme(RichText description) : ILexemeItem {
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public IList<int> Path { get; set; } = [1];
    
    public RichText Description { get; set; } = description;
}

public static class LexemeExtensions {
    public static Lexeme ToDomain(this LexemeDbo dbo) {
        return new Lexeme(new RichText(dbo.Description ?? string.Empty)) {
            Id = dbo.Id,
            Path =  dbo.Path?.ToList() ?? [1],
        };
    }
}