using YordanApi;
using Yordanew.Models;
using Yordanew.Domain.ValueObjects;

namespace Yordanew.Domain.Entity;

public class Language {
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required Translatable Name { get; set; }
    public RichText Description { get; set; } = new RichText("");
    public bool IsPublished { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public required Guid AuthorId { get; set; }
    public IEnumerable<Guid> EditorsIds { get; set; } = new List<Guid>();
    public IEnumerable<PartOfSpeech> PartsOfSpeech { get; set; } = new List<PartOfSpeech>();
    public IEnumerable<Article> Articles { get; set; } = new List<Article>();
    
    public static Language Create(Guid authorId, Translatable name, bool isPublished = false) {
        return new Language { Name = name, IsPublished = isPublished, AuthorId = authorId};
    }
    
    public void AddEditor(Guid editorId) {
        EditorsIds = EditorsIds.Append(editorId);
    }
    
    public void AppendPartOfSpeech(PartOfSpeech partOfSpeech) {
        PartsOfSpeech = PartsOfSpeech.Append(partOfSpeech);
    }
    
    public void RemovePartOfSpeech(Guid partOfSpeechId) {
        PartsOfSpeech = PartsOfSpeech.Where(pos => pos.Id != partOfSpeechId);
    }

    public void ReorderPartOfSpeech(Guid partOfSpeechId, int newIndex) {
        if (newIndex >= PartsOfSpeech.Count())
            return;
        
        var listWithout = PartsOfSpeech.Where(pos => pos.Id != partOfSpeechId);
        var added = listWithout.InsertAfter(newIndex - 1, PartsOfSpeech.Single(pos => pos.Id == partOfSpeechId));
        PartsOfSpeech = added;
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
        };
    }

    public void Update(string? name, string? autoName, string? autoNameTranscription, bool? isPublished, string? description) {
        Name = new Translatable(name ?? Name.Content, autoName ?? Name.Translation, autoNameTranscription ?? Name.Transcription);
        Description = new RichText(description ?? Description.Content);
        IsPublished = isPublished ?? IsPublished;
    }
}

public static class LanguageExtensions {
    public static Language ToDomain(this LanguageDbo dbo) {
        return new Language {
            Id = dbo.Id,
            AuthorId = dbo.AuthorId,
            Name = new Translatable(dbo.Name ?? string.Empty, dbo.AutoName, dbo.AutoNameTranscription),
            Description = new RichText(dbo.Description ?? string.Empty),
            IsPublished = dbo.IsPublished,
            CreatedAt = dbo.CreatedAt,
            EditorsIds = dbo.EditorsIds?.ToList() ?? [],
            // TODO pos, articles
        };
    }
}