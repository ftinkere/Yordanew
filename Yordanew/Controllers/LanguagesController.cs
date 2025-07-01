using System.ComponentModel.DataAnnotations;
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

public class LanguagesController(
    UserManager<AppUser> userManager,
    LanguageService languageService
) : Controller {
    [HttpGet("/languages")]
    public async Task<IActionResult> Index() {
        var user = GetCurrentUser();
        
        var publishedLanguages = (await languageService.GetPublished()).Select(l => l.ToDto());
        var ownLanguages = user is null ? new List<LanguageDto>() : (await languageService.GetOwn(user.Id)).Select(l => l.ToDto());
        return Inertia.Render("Languages/Index", new {
            PublishedLanguages = publishedLanguages,
            OwnLanguages = ownLanguages,
        });
    }
    
    [Authorize]
    [HttpGet("/languages/create")]
    public IActionResult Create() {
        return Inertia.Render("Languages/Edit");
    }

    public class LanguageEditRequest {
        public Guid? Id { get; set; } = null;
        [Required(ErrorMessage = "Обязательное поле")]
        public required string Name { get; set; }
        public string? Autoname { get; set; }
        public string? AutonameTranscription { get; set; }
        public bool IsPublished { get; set; } = false;
        public string Description { get; set; } = "";
    }

    [Authorize]
    [HttpPost("/languages/edit")]
    public async Task<IActionResult> EditPost([FromBody] LanguageEditRequest request) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();

        if (request.Id is null) {
            var language = Language.Create(user.Id,
                new Translatable(request.Name, request.Autoname, request.AutonameTranscription),
                request.IsPublished);
            language.Description = new RichText(request.Description);
            await languageService.Insert(language);
            return Inertia.Location($"/languages/{language.Id}");
        }

        var existing = await languageService.GetById(request.Id.Value);
        if (existing is null) return NotFound();
        if (existing.AuthorId != user.Id) return Unauthorized();

        existing.Update(request.Name, request.Autoname, request.AutonameTranscription,
            request.IsPublished, request.Description);

        await languageService.Update(existing);
        return Inertia.Location($"/languages/{existing.Id}");
    }

    [HttpGet("/languages/{id}")]
    public async Task<IActionResult> View(Guid id) {
        var language = (await languageService.GetById(id));
        if (language is null) return NotFound();
        return Inertia.Render("Languages/View", new {
            Language = language.ToDto()
        });
    }

    [Authorize]
    [HttpGet("/languages/{id}/edit")]
    public async Task<IActionResult> Edit(Guid id) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var language = await languageService.GetById(id);
        if (language is null) return NotFound();
        if (user.Id != language.AuthorId) return Unauthorized();
        
        return Inertia.Render("Languages/Edit", new {
            Language = language.ToDto()
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