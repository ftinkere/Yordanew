using System.ComponentModel.DataAnnotations;
using System.Text.Json;
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

public class DictionaryController(
    UserManager<AppUser> userManager,
    LanguageService languageService,
    DictionaryService dictionaryService,
    FileService fileService
) : Controller {
    
    [HttpGet("/languages/{id:guid}/dictionary")]
    public async Task<IActionResult> Index(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) {
        var language = await languageService.GetFullById(id, page, pageSize);
        if (language is null) return NotFound();
        
        return Inertia.Render("Dictionary/Index", new {
            Language = language.ToDto(page, pageSize, await languageService.CountArticles(language.Id) )
        });
    }
    
    [Authorize]
    [HttpGet("/languages/{id:guid}/dictionary/create")]
    public async Task<IActionResult> Create(Guid id) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var language = await languageService.GetById(id);
        if (language is null) return NotFound();
        if (user.Id != language.AuthorId) return Unauthorized();
        
        return Inertia.Render("Dictionary/Create", new {
            Language = language.ToDto()
        });
    }

    public class CreateLexemeRequest {
        public Guid? Id { get; set; }
        public string? Path { get; set; } = "1.1";
        public string? Article { get; set; }
    }

    public class CreateDictionaryRequest {
        public Guid? Id { get; set; }
        [Required(ErrorMessage = "Поле обязательное"), MinLength(1)]
        public required string Vocabula { get; set; }
        public string? Transcription { get; set; }
        public string? Adaptation { get; set; }
        
        public IList<Guid>? AddFiles { get; set; } = new List<Guid>();
        public IList<CreateLexemeRequest> Lexemes { get; set; } = new List<CreateLexemeRequest>();
    }
    
    [Authorize]
    [HttpPost("/languages/{id:guid}/dictionary/create")]
    public async Task<IActionResult> Store(Guid id, [FromBody] CreateDictionaryRequest request) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var language = await languageService.GetById(id);
        if (language is null) return NotFound();
        if (user.Id != language.AuthorId) return Unauthorized();

        if (ModelState.IsValid) {
            var article = new Article(new Transcriptable(request.Vocabula, request.Transcription, request.Adaptation)) {
                LanguageId = language.Id,
            };
            foreach (var requestLexeme in request.Lexemes) {
                var indexes = requestLexeme.Path?.Split('.').Select(int.Parse) ?? [];
                var lexeme = new Lexeme {
                    ArticleId = article.Id,
                    Description = new RichText(requestLexeme.Article ?? string.Empty),
                    Path = indexes.ToList()
                };
                article.AddLexeme(lexeme);
            }

            await dictionaryService.Insert(article);
            foreach (var fileId in request.AddFiles ?? new List<Guid>()) {
                await fileService.LinkFile(fileId, article.Id, nameof(ArticleDbo));
            }
            return Inertia.Location($"/dictionary/{article.Id}");
        }
        
        return Inertia.Render("Dictionary/Create", new {
            Language = language.ToDto()
        });
    }
    
    [HttpGet("/dictionary/{id:guid}")]
    public async Task<IActionResult> View(Guid id) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var article = await dictionaryService.GetById(id);
        if (article is null) return NotFound();
        var language = await languageService.GetById(article.LanguageId);
        if (language is null) return NotFound();
        if (user.Id != language.AuthorId && !language.IsPublished) return Unauthorized();
        
        return Inertia.Render("Dictionary/View", new {
            Language = language.ToDto(),
            Article = article.ToDto(),
        });
    }
    
    [HttpGet("/dictionary/{id:guid}/edit")]
    public async Task<IActionResult> Edit(Guid id) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var article = await dictionaryService.GetById(id);
        if (article is null) return NotFound();
        var language = await languageService.GetById(article.LanguageId);
        if (language is null) return NotFound();
        if (user.Id != language.AuthorId && !language.IsPublished) return Unauthorized();
        
        return Inertia.Render("Dictionary/Edit", new {
            Language = language.ToDto(),
            Article = article.ToDto(),
        });
    }
    
    [HttpPost("/dictionary/{id:guid}/edit")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateDictionaryRequest request) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var earticle = await dictionaryService.GetById(id);
        if (earticle is null) return NotFound();
        var language = await languageService.GetById(earticle.LanguageId);
        if (language is null) return NotFound();
        if (user.Id != language.AuthorId && !language.IsPublished) return Unauthorized();
        
        if (ModelState.IsValid) {
            var article = new Article(new Transcriptable(request.Vocabula, request.Transcription, request.Adaptation)) {
                Id = earticle.Id,
                LanguageId = earticle.LanguageId,
                Lexemes = earticle.Lexemes.ToList()
            };
            foreach (var requestLexeme in request.Lexemes) {
                var indexes = requestLexeme.Path?.Split('.').Select(int.Parse) ?? [];
                var lexeme = new Lexeme {
                    Id = requestLexeme.Id ?? Guid.CreateVersion7(),
                    ArticleId = article.Id,
                    Description = new RichText(requestLexeme.Article ?? ""),
                    Path = indexes.ToList()
                };
                article.AddLexeme(lexeme);
            }

            await dictionaryService.Update(article);
            foreach (var fileId in request.AddFiles ?? new List<Guid>()) {
                await fileService.LinkFile(fileId, article.Id, nameof(ArticleDbo));
            }
            return Inertia.Location($"/dictionary/{article.Id}");
        }
        
        return Inertia.Location($"/dictionary/{id}/edit");
    }
    
    private AppUser? GetCurrentUser() {
        var name = HttpContext.User.Identity?.Name;
        if (name is null) {
            return null;
        }
        return userManager.Users.FirstOrDefault(u => u.UserName == name);
    }
    
    [Authorize]
    [HttpGet("/dictionary/files")]
    public IActionResult FetchFile([FromQuery] Guid id) {
        return PhysicalFile(Path.Combine(fileService.GetBasePath("dictionary"), id.ToString()), "application/octet-stream");
    }
    
    
    [Authorize]
    [HttpPost("/dictionary/files")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file) {
        return Ok((await fileService.UploadFilepondFile(file, "dictionary")).ToString());
    }
    
    [Authorize]
    [HttpDelete("/dictionary/files")]
    public async Task<IActionResult> UploadFileRevert() {
        using var reader = new StreamReader(Request.Body);
        var rawBody = await reader.ReadToEndAsync();

        // Убрать внешние кавычки и экранирование
        var guid = JsonSerializer.Deserialize<string>(rawBody);
        if (guid is null) return BadRequest();
        fileService.DeleteFilepondFile(Guid.Parse(guid), "dictionary");
        return Ok();
    }

    [Authorize]
    [HttpPost("/dictionary/{id:guid}/files")]
    public async Task<IActionResult> AttachFile(Guid id, [FromBody] Guid fileId) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var article = await dictionaryService.GetById(id);
        if (article is null) return NotFound();
        var language = await languageService.GetById(article.LanguageId);
        if (language is null || language.AuthorId != user.Id) return Unauthorized();
        await fileService.LinkFile(fileId, id, nameof(ArticleDbo));
        return Ok();
    }

    [Authorize]
    [HttpDelete("/dictionary/{id:guid}/files/{fileId:guid}")]
    public async Task<IActionResult> RemoveFile(Guid id, Guid fileId) {
        var user = GetCurrentUser();
        if (user is null) return Unauthorized();
        var article = await dictionaryService.GetById(id);
        if (article is null) return NotFound();
        var language = await languageService.GetById(article.LanguageId);
        if (language is null || language.AuthorId != user.Id) return Unauthorized();
        fileService.UnlinkFile(fileId, id);
        return Ok();
    }
}