using LinqToDB;
using LinqToDB.Data;
using Yordanew.Models;
using Yordanew.Domain.Entity;

namespace Yordanew.Services;

public class DictionaryService(AppDbContext db) {
    public async Task<Article> Insert(Article article) {
        return (await db.Articles.AddAsync(new ArticleDbo {
            Id = article.Id,
            LanguageId = article.LanguageId,
            Lemma = article.Lemma.Content,
            Transcription = article.Lemma.Transcription,
            Adaptation = article.Lemma.Adaptation
        })).Entity.ToDomain();
    }

    public async Task<Article?> GetById(Guid id) {
        return (await db.Articles.FirstOrDefaultAsync(a => a.Id == id))?.ToDomain();
    }

    public async Task<Article> Update(Article article) {
        var dbo = await db.Articles.FirstOrDefaultAsync(a => a.Id == article.Id);
        if (dbo is null) {
            return await Insert(article);
        }
        dbo.Lemma = article.Lemma.Content;
        dbo.Transcription = article.Lemma.Transcription;
        dbo.Adaptation = article.Lemma.Adaptation;
        await db.SaveChangesAsync();
        return dbo.ToDomain();
    }
}