using Microsoft.EntityFrameworkCore;
using Yordanew.Domain.Entity;
using Yordanew.Models;

namespace Yordanew.Services;

public class GrammaticService(
    AppDbContext db
) {
    public async Task<PartOfSpeech?> GetPartOfSpeech(Guid? id) {
        if (id is null) {
            return null;
        }

        var pos = await db.PartsOfSpeech
            .Where(pos => pos.Id == id)
            .FirstOrDefaultAsync();

        if (pos is not null) {
            db.Entry(pos).State = EntityState.Detached;
        }
        
        return pos?.ToDomain();
    }

    public async Task<PartOfSpeech> StorePartOfSpeech(PartOfSpeech partOfSpeech) {
        var pos = await GetPartOfSpeech(partOfSpeech.Id);
        var dbo = new PartOfSpeechDbo {
            Id = partOfSpeech.Id,
            LanguageId = partOfSpeech.LanguageId,
            Name = partOfSpeech.Name.Value,
            Code = partOfSpeech.Code.Value,
            Description = partOfSpeech.Description.Content,
        };
        if (pos is null) {
            db.PartsOfSpeech.Add(dbo);
        } else {
            db.PartsOfSpeech.Update(dbo);
        }

        await db.SaveChangesAsync();
        return partOfSpeech;
    }

    public async Task<GrammaticalCategory?> GetCategory(Guid? id) {
        if (id is null) {
            return null;
        }

        var cat = await db.Categories
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (cat is not null) {
            db.Entry(cat).State = EntityState.Detached;
        }
        
        return cat?.ToDomain();
    }

    public async Task<GrammaticalCategory> StoreCategory(GrammaticalCategory category) {
        var cat = await GetCategory(category.Id);
        var dbo = new GrammaticalCategoryDbo {
            Id = category.Id,
            PartOfSpeechId = category.PosId,
            Name = category.Name.Value,
            Code = category.Code.Value,
            Description = category.Description.Content,
        };
        if (cat is null) {
            db.Categories.Add(dbo);
        }
        else {
            db.Categories.Update(dbo);
        }

        await db.SaveChangesAsync();
        return category;
    }

    public async Task<GrammaticalFeature?> GetFeature(Guid? id) {
        if (id is null) {
            return null;
        }

        var feat = await db.Features
            .Where(f => f.Id == id)
            .FirstOrDefaultAsync();

        if (feat is not null) {
            db.Entry(feat).State = EntityState.Detached;
        }
        return feat?.ToDomain();
    }

    public async Task<GrammaticalFeature> StoreFeature(GrammaticalFeature feature) {
        var feat = await GetFeature(feature.Id);
        var dbo = new GrammaticalFeatureDbo {
            Id = feature.Id,
            CategoryId = feature.CategoryId,
            Name = feature.Name.Value,
            Code = feature.Code.Value,
            Description = feature.Description.Content,
        };
        if (feat is null) {
            db.Features.Add(dbo);
        }
        else {
            db.Features.Update(dbo);
        }

        await db.SaveChangesAsync();
        return feature;
    }
}