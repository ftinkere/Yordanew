using Microsoft.EntityFrameworkCore;
using Yordanew.Models;
using Yordanew.Domain.Entity;

namespace Yordanew.Services;

public class LanguageService(AppDbContext db) {
    public async Task<Language> Insert(Language language) {
        // TODO: articles and other
        var ret = (await db.Languages.AddAsync(new LanguageDbo {
            Id = language.Id,
            AuthorId = language.AuthorId,
            Name = language.Name.Content,
            AutoName = language.Name.Translation,
            AutoNameTranscription = language.Name.Transcription,
            Description = language.Description.Content,
            IsPublished = language.IsPublished,
            CreatedAt = language.CreatedAt,
            // EditorsIds = language.EditorsIds.ToArray(),
        })).Entity.ToDomain();

        await db.SaveChangesAsync();
        
        return ret;
    }

    public async Task<Language?> GetById(Guid id) {
        var query = db.Languages
            .Include(l => l.Author)
            .AsQueryable();

        var entity = await query.FirstOrDefaultAsync(l => l.Id == id);
        return entity?.ToDomain();
    }

    public async Task<Language?> GetFullById(Guid id, int page = 1, int pageSize = 10) {
        var query = db.Languages
            .Include(l => l.Author); 
            
        var entity = await query.FirstOrDefaultAsync(l => l.Id == id);

        if (entity is null) return null;
        
        await db.Entry(entity)
            .Collection(l => l.Articles)
            .Query()
            .Include(a => a.Lexemes)
            .OrderBy(a => a.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .LoadAsync();
        
        
        await db.Entry(entity)
            .Collection(l => l.PartsOfSpeech)
            .Query()
            .OrderBy(pos => pos.Id)
            .Include(pos => pos.Categories)
            .ThenInclude(c => c.Features)
            .LoadAsync();

        foreach (var pos in entity.PartsOfSpeech)
        {
            pos.Categories = pos.Categories
                .OrderBy(c => c.Id)
                .ToList();

            foreach (var cat in pos.Categories)
            {
                cat.Features = cat.Features
                    .OrderBy(f => f.Id)
                    .ToList();
            }
        }
        
        return entity.ToDomain();
    }

    public Task<int> CountArticles(Guid languageId) {
        return db.Articles.CountAsync(a => a.LanguageId == languageId);
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
        // dbo.EditorsIds = language.EditorsIds.ToArray();
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