using InertiaCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yordanew.Domain.Entity;
using Yordanew.Domain.ValueObjects;
using Yordanew.Dtos;
using Yordanew.Models;
using Yordanew.Services;

namespace Yordanew.Controllers;

public class GrammaticController(
    UserManager<AppUser> userManager,
    LanguageService languageService,
    GrammaticService grammaticService
) : Controller {
    
    [HttpGet("/grammatic/{id:guid}")]
    public async Task<IActionResult> Index(Guid id) {
        var language = await languageService.GetFullById(id);
        
        return Inertia.Render("Grammatic/Index", new {
            language = language?.ToDto(),
        });
    }

    [Authorize]
    [HttpGet("/grammatic/{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id) {
        var language = await languageService.GetFullById(id);
        return Inertia.Render("Grammatic/Edit", new {
            language = language?.ToDto(),
        });
    }

    [Authorize]
    [HttpPost("/grammatic/{id:guid}/edit")]
    public async Task<IActionResult> Store(Guid id) {
        var language = await languageService.GetFullById(id);
        return Inertia.Render("Grammatic/Edit", new {
            language = language?.ToDto(),
        });
    }

    [Authorize]
    [HttpGet("/grammatic/parts-of-speech/{id:guid}")]
    public async Task<IActionResult> EditPos(Guid id) {
        var pos = await grammaticService.GetPartOfSpeech(id);
        if (pos is null) return NotFound();
        var language = await languageService.GetFullById(pos.LanguageId);
        return Inertia.Render("Grammatic/Edit", new {
            language = language?.ToDto(),
            partOfSpeech = pos.ToDto(),
        });
    }
    
    public class PosStoreRequest {
        public Guid? Id { get; set; }
        public Guid? LanguageId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
    
    [Authorize]
    [HttpPost("/grammatic/parts-of-speech/{id:guid?}")]
    public async Task<IActionResult> EditPosPost(Guid? id, [FromBody] PosStoreRequest request) {
        var authorId = GetCurrentUser()?.Id;
        if (authorId is null) {
            return Unauthorized();
        }
        
        var posId = id ?? request.Id;
        var pos = await grammaticService.GetPartOfSpeech(posId ?? request.Id);
        var language = await languageService.GetById(pos?.LanguageId ?? request.LanguageId ?? Guid.Empty);
        if (language is null) return BadRequest("LanguageId is required.");

        if (language.AuthorId != authorId) {
            return Unauthorized();
        }

        var name = request.Name ?? pos?.Name.Value;
        var code = request.Code ?? pos?.Code.Value;
        if (name is null || code is null) {
            return BadRequest("Name and code are required.");
        }
        
        var newPos = new PartOfSpeech {
            Id = posId ?? Guid.CreateVersion7(),
            LanguageId = language.Id,
            Name = new Name(name),
            Code = new Code(code),
            Description = new RichText(request.Description ?? pos?.Description.Content ?? string.Empty),
        };

        await grammaticService.StorePartOfSpeech(newPos);
        
        var updatedLanguage = await languageService.GetFullById(language.Id);
        return Inertia.Render("Grammatic/Edit", new {
            language = updatedLanguage?.ToDto(),
            partOfSpeech = newPos.ToDto(),
        });
    }

    [Authorize]
    [HttpGet("/grammatic/categories/{id:guid}")]
    public async Task<IActionResult> EditCategory(Guid id) {
        var category = await grammaticService.GetCategory(id);
        if (category is null) return NotFound();
        var pos = await grammaticService.GetPartOfSpeech(category.PosId);
        var language = await languageService.GetFullById(pos!.LanguageId);
        return Inertia.Render("Grammatic/Edit", new {
            language = language?.ToDto(),
            category = category.ToDto(),
            partOfSpeech = pos.ToDto(),
        });
    }
    
    public class CategoryStoreRequest {
        public Guid? Id { get; set; }
        public Guid? PosId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
    
    [Authorize]
    [HttpPost("/grammatic/categories/{id:guid?}")]
    public async Task<IActionResult> EditCategoryPost(Guid? id, [FromBody] CategoryStoreRequest request) {
        var authorId = GetCurrentUser()?.Id;
        if (authorId is null) {
            return Unauthorized();
        }
        
        var catId = id ?? request.Id;
        var category = await grammaticService.GetCategory(catId);
        var pos = await grammaticService.GetPartOfSpeech(category?.PosId ?? request.PosId);
        if (pos is null) return BadRequest("PosId is required.");
        var language = await languageService.GetById(pos.LanguageId);

        if (language?.AuthorId != authorId) {
            return Unauthorized();
        }

        var name = request.Name ?? category?.Name.Value;
        var code = request.Code ?? category?.Code.Value;
        if (name is null || code is null) {
            return BadRequest("Name and code are required.");
        }
        
        var newCategory = new GrammaticalCategory {
            Id = catId ?? Guid.CreateVersion7(),
            PosId = pos.Id,
            Name = new Name(name),
            Code = new Code(code),
            Description = new RichText(request.Description ?? category?.Description.Content ?? string.Empty),
        };

        await grammaticService.StoreCategory(newCategory);
        
        var updatedLanguage = await languageService.GetFullById(language.Id);
        return Inertia.Render("Grammatic/Edit", new {
            language = updatedLanguage?.ToDto(),
            category = newCategory.ToDto(),
            partOfSpeech = pos.ToDto(),
        });
    }

    [Authorize]
    [HttpGet("/grammatic/features/{id:guid}")]
    public async Task<IActionResult> EditFeature(Guid id) {
        var feature = await grammaticService.GetFeature(id);
        if (feature is null) return NotFound();
        var category = await grammaticService.GetCategory(feature.CategoryId);
        var pos = await grammaticService.GetPartOfSpeech(category!.PosId);
        var language = await languageService.GetFullById(pos!.LanguageId);
        return Inertia.Render("Grammatic/Edit", new {
            language = language?.ToDto(),
            feature = feature.ToDto(),
            category = category.ToDto(),
            partOfSpeech = pos.ToDto(),
        });
    }

    public class FeatureStoreRequest {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
    
    [Authorize]
    [HttpPost("/grammatic/features/{id:guid?}")]
    public async Task<IActionResult> EditFeaturePost(Guid? id, [FromBody] FeatureStoreRequest request) {
        var authorId = GetCurrentUser()?.Id;
        if (authorId is null) {
            return Unauthorized();
        }
        
        var featId = id ?? request.Id;
        var feature = await grammaticService.GetFeature(featId);
        var category = await grammaticService.GetCategory(feature?.CategoryId ?? request.CategoryId);
        if (category is null) return BadRequest("CategoryId is required.");
        var pos = await grammaticService.GetPartOfSpeech(category.PosId);
        var language = await languageService.GetById(pos!.LanguageId);

        if (language?.AuthorId != authorId) {
            return Unauthorized();
        }
        
        var name = request.Name ?? feature?.Name.Value;
        var code = request.Code ?? feature?.Code.Value;
        if (name is null || code is null) {
            return BadRequest("Name and code are required.");
        }

        var newFeature = new GrammaticalFeature {
            Id = featId ?? Guid.CreateVersion7(),
            CategoryId = category.Id,
            Name = new Name(name),
            Code = new Code(code),
            Description = new RichText(request.Description ?? feature?.Description.Content ?? string.Empty),
        };

        await grammaticService.StoreFeature(newFeature);
        
        var updatedLanguage = await languageService.GetFullById(language.Id);
        return Inertia.Render("Grammatic/Edit", new {
            language = updatedLanguage?.ToDto(),
            feature = newFeature.ToDto(),
            category = category.ToDto(),
            partOfSpeech = pos.ToDto(),
        });
    }
    
    private AppUser? GetCurrentUser() {
        var name = HttpContext.User.Identity?.Name;
        if (name is null) {
            return null;
        }
        return userManager.Users.FirstOrDefault(u => u.UserName == name);
    }
}