using System.ComponentModel.DataAnnotations;
using InertiaCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yordanew.Domain.Entity;
using Yordanew.Domain.ValueObjects;
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
        
        var publishedLanguages = await languageService.GetPublished();
        var ownLanguages = user is null ? new List<Language>() : (await languageService.GetOwn(user.Id));
        return Inertia.Render("Languages/Index", new {
            PublishedLanguages = publishedLanguages,
            OwnLanguages = ownLanguages,
        });
    }
    
    [Authorize]
    [HttpGet("/languages/create")]
    public IActionResult Create() {
        return Inertia.Render("Languages/Create");
    }

    public class CreateLanguageRequest {
        [Required(ErrorMessage = "Обязательное поле")]
        public required string Name { get; set; }
        public string? Autoname { get; set; }
        public string? AutonameTranscription { get; set; }
        public bool IsPublished { get; set; } = false;
    }
    
    [Authorize]
    [HttpPost("/languages/create")]
    public async Task<IActionResult> Store([FromBody] CreateLanguageRequest request) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var language = Language.Create(user.Id, new Translatable(request.Name, request.Autoname, request.AutonameTranscription), request.IsPublished);
        var answer = await languageService.Insert(language);
        return Inertia.Location($"/languages/{language.Id}");
    }

    [HttpGet("/languages/{id}")]
    public async Task<IActionResult> View(Guid id) {
        var language = await languageService.GetById(id);
        if (language is null) return NotFound();
        return Inertia.Render("Languages/View", new {
            Language = language
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
            Language = language
        });
    }

    public class LanguageUpdateRequest {
        [Required(ErrorMessage = "Обязательное поле")]
        public required string Name { get; set; }
        public string? Autoname { get; set; }
        public string? AutonameTranscription { get; set; }
        public bool IsPublished { get; set; } = false;
        public string Description { get; set; } = "";
    }
    
    [Authorize]
    [HttpPost("/languages/{id}/edit")]
    public async Task<IActionResult> EditPost(Guid id, [FromBody] LanguageUpdateRequest request) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var language = await languageService.GetById(id);
        if (language is null) return NotFound();
        if (user.Id != language.AuthorId) return Unauthorized();
        
        if (ModelState.IsValid) {
            language.Update(request.Name, request.Autoname, request.AutonameTranscription, request.IsPublished,
                request.Description);

            var res = await languageService.Update(language); 
            return Inertia.Location($"/languages/{language.Id}");
        }
        
        return Inertia.Render("Languages/Edit", new {
            Language = language
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