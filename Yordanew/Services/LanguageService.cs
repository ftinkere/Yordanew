using LinqToDB;
using Yordanew.Models;
using Yordanew.Domain.Entity;

namespace Yordanew.Services;

public class LanguageService(AppDbContext db) {
    public async Task<Language> Insert(Language language) {
        // TODO: articles and other
        return (await db.Languages.AddAsync(new LanguageDbo {
            Id = language.Id,
            AuthorId = language.AuthorId,
            Name = language.Name.Content,
            AutoName = language.Name.Translation,
            AutoNameTranscription = language.Name.Transcription,
            Description = language.Description.Content,
            IsPublished = language.IsPublished,
            CreatedAt = language.CreatedAt,
            EditorsIds = language.EditorsIds.ToArray(),
        })).Entity.ToDomain();
    }

    public async Task<Language?> GetById(Guid id) {
        return (await db.Languages.FirstOrDefaultAsync(l => l.Id == id))?.ToDomain();
    }

    public async Task<Language> Update(Language language) {
        var dbo = await db.Languages.FirstOrDefaultAsync(l => l.Id == language.Id);
        if (dbo is null) {
            return await Insert(language);
        }
        dbo.Name = language.Name.Content;
        dbo.AutoName = language.Name.Translation;
        dbo.AutoNameTranscription = language.Name.Transcription;
        dbo.Description = language.Description.Content;
        dbo.IsPublished = language.IsPublished;
        dbo.EditorsIds = language.EditorsIds.ToArray();
        await db.SaveChangesAsync();
        return dbo.ToDomain();
    }

    public Task<List<Language>> GetPublished() {
        return db.Languages.Where(l => l.IsPublished).Select(l => l.ToDomain()).ToListAsync();
    }

    public Task<List<Language>> GetOwn(Guid userId) {
        return db.Languages.Where(l => l.AuthorId == userId).Select(l => l.ToDomain()).ToListAsync();
    }
}