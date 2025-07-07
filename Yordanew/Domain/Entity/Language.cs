using YordanApi;
using Yordanew.Models;
using Yordanew.Domain.ValueObjects;
using Yordanew.Dtos;

namespace Yordanew.Domain.Entity;

public class Language {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required Translatable Name { get; set; }
    public RichText Description { get; set; } = new RichText("");
    public bool IsPublished { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public required Guid AuthorId { get; set; }
    
    public IEnumerable<Article> Articles { get; set; } = new List<Article>();
    
    public IDictionary<PartOfSpeech, IDictionary<GrammaticalCategory, IList<GrammaticalFeature>>> Grammatic { get; set; } = new Dictionary<PartOfSpeech, IDictionary<GrammaticalCategory, IList<GrammaticalFeature>>>();
    
    public Author Author { get; set; }
    
    
    public static Language Create(Guid authorId, Translatable name, bool isPublished = false) {
        return new Language { Name = name, IsPublished = isPublished, AuthorId = authorId};
    }
    
    public void AddArticle(Article article) {
        Articles = Articles.Append(article);
    }

    public Language ToLight() {
        return new Language {
            Id = Id,
            AuthorId = AuthorId,
            Name = Name,
            Description = Description,
            IsPublished = IsPublished,
            Author = Author,
        };
    }

    public void Update(string? name, string? autoName, string? autoNameTranscription, bool? isPublished, string? description) {
        Name = new Translatable(name ?? Name.Content, autoName ?? Name.Translation, autoNameTranscription ?? Name.Transcription);
        Description = new RichText(description ?? Description.Content);
        IsPublished = isPublished ?? IsPublished;
    }

    public void FillGrammatic(IEnumerable<PartOfSpeechDbo> posDbos) {
        Grammatic = new Dictionary<PartOfSpeech, IDictionary<GrammaticalCategory, IList<GrammaticalFeature>>>();
        foreach (var posDbo in posDbos) {
            var pos = posDbo.ToDomain();
            var dict = new Dictionary<GrammaticalCategory, IList<GrammaticalFeature>>();
            foreach (var category in posDbo.Categories) {
                var cat = category.ToDomain();
                dict[cat] = category.Features.Select(f => f.ToDomain()).ToList();;
            }
            Grammatic[pos] = dict;
        }
    }
    
    public override bool Equals(object? obj) {
        if (obj is Language pos) {
            return Id.Equals(pos.Id);
        }
        return false;
    }

    public override int GetHashCode() {
        return Id.GetHashCode();
    }
}

public static class LanguageExtensions {
    public static Language ToDomain(this LanguageDbo dbo) {
        var lang = new Language {
            Id = dbo.Id,
            AuthorId = dbo.AuthorId,
            Name = new Translatable(dbo.Name ?? string.Empty, dbo.AutoName, dbo.AutoNameTranscription),
            Description = new RichText(dbo.Description ?? string.Empty),
            IsPublished = dbo.IsPublished,
            CreatedAt = dbo.CreatedAt,
            Author = dbo.Author.ToDomain(),
            Articles = dbo.Articles.Select(a => a.ToDomain()).ToList(),
        };
        lang.FillGrammatic(dbo.PartsOfSpeech);
        return lang;
    }
}