using Microsoft.EntityFrameworkCore;
using Yordanew.Models;
using Yordanew.Domain.Entity;

namespace Yordanew.Services;

public class DictionaryService(AppDbContext db) {
    public async Task<Article> Insert(Article article) {
        await db.Articles.AddAsync(new ArticleDbo {
            Id = article.Id,
            LanguageId = article.LanguageId,
            Lemma = article.Lemma.Content,
            Transcription = article.Lemma.Transcription,
            Adaptation = article.Lemma.Adaptation
        });

        foreach (var lexeme in article.Lexemes) {
            await db.Lexemes.AddAsync(new LexemeDbo {
                Id = lexeme.Id,
                ArticleId = article.Id,
                Description = lexeme.Description.Content,
                Path = lexeme.Path.ToArray(),
            });
        }
        
        await db.SaveChangesAsync();
        
        return (await GetById(article.Id))!;
    }

    public async Task<Article?> GetById(Guid id) {
        return (await db.Articles
            .Include(a => a.Lexemes)
            .FirstOrDefaultAsync(a => a.Id == id))?.ToDomain();
    }

    public async Task<Article> Update(Article article) {
        var dbo = await db.Articles.FirstOrDefaultAsync(a => a.Id == article.Id);
        if (dbo is null) {
            return await Insert(article);
        }
        dbo.Lemma = article.Lemma.Content;
        dbo.Transcription = article.Lemma.Transcription;
        dbo.Adaptation = article.Lemma.Adaptation;

        foreach (var lexeme in article.Lexemes) {
            var dboLexeme = dbo.Lexemes.FirstOrDefault(l => l.Id == lexeme.Id);
            if (dboLexeme is null) {
                await db.Lexemes.AddAsync(new LexemeDbo {
                    Id = lexeme.Id,
                    ArticleId = article.Id,
                    Description = lexeme.Description.Content,
                    Path = lexeme.Path.ToArray(),
                });
            } else {
                dboLexeme.Description = lexeme.Description.Content;
                dboLexeme.Path = lexeme.Path.ToArray();
            }
        }
        
        await db.SaveChangesAsync();
        return (await GetById(article.Id))!;
    }
}