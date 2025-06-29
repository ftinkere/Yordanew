using System.ComponentModel.DataAnnotations;
using InertiaCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yordanew.Domain.Entity;
using Yordanew.Dtos;
using Yordanew.Models;
using Yordanew.Services;

namespace Yordanew.Controllers;

public class ProfileController(
    UserManager<AppUser> userManager,
    LanguageService languageService,
    FileService fileService
) : Controller {
    [Authorize]
    [HttpGet("/profile")]
    public async Task<IActionResult> Index() {
        var user = await GetCurrentUser();
        if (user is null) return Unauthorized();
        var languages = await languageService.GetOwn(user.Id);
        return Inertia.Render("Profile/Index", new {
            User = user.ToDomain().ToDto(),
            Languages = languages.Select(l => l.ToDto())
        });
    }

    [Authorize]
    [HttpGet("/profile/edit")]
    public async Task<IActionResult> Edit() {
        var user = await GetCurrentUser();
        if (user is null) return Unauthorized();
        return Inertia.Render("Profile/Edit", new {
            User = user.ToDomain().ToDto()
        });
    }

    public class EditRequest {
        [Required(ErrorMessage = "Обязательное поле")]
        public required string DisplayName { get; set; }
    }

    [Authorize]
    [HttpPost("/profile/edit")]
    public async Task<IActionResult> EditPost([FromForm] EditRequest request, IFormFile? avatar) {
        var user = await GetCurrentUser();
        if (user is null) return Unauthorized();

        if (ModelState.IsValid) {
            user.DisplayName = request.DisplayName;
            await userManager.UpdateAsync(user);
            if (avatar is not null) {
                fileService.UploadAvatar(avatar, user.Id);
            }
            return Inertia.Location("/profile");
        }

        return Inertia.Render("Profile/Edit", new {
            User = user.ToDomain().ToDto()
        });
    }

    [HttpGet("/profile/{id:guid}")]
    public async Task<IActionResult> View(Guid id) {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return NotFound();
        var languages = await languageService.GetOwn(id);
        return Inertia.Render("Profile/View", new {
            User = user.ToDomain().ToDto(),
            Languages = languages.Select(l => l.ToDto())
        });
    }

    private Task<AppUser?> GetCurrentUser() {
        var name = HttpContext.User.Identity?.Name;
        return name is null
            ? Task.FromResult<AppUser?>(null)
            : userManager.Users.FirstOrDefaultAsync(u => u.UserName == name);
    }
}
