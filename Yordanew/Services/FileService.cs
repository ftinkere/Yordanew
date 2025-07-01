using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Yordanew.Models;

namespace Yordanew.Services;

public class FileService(
    IWebHostEnvironment env, 
    AppDbContext db
    ) {
    public string GetAvatarPath(Guid userId) {
        return Path.Combine(env.WebRootPath, "uploaded-files", "avatars", userId.ToString());
    }
    public string GetBasePath(string folder) {
        return Path.Combine(env.WebRootPath, "uploaded-files", folder);
    }

    public async Task<bool> UploadAvatar(IFormFile file, Guid userId) {
        if (file.Length <= 0 || !file.ContentType.StartsWith("image/")) {
            return false;
        }

        var path = GetAvatarPath(userId);
        Directory.CreateDirectory(Path.Combine(env.WebRootPath, "uploaded-files", "avatars"));

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(stream);

        return true;
    }

    public bool DeleteFilepondFile(Guid file, string folder) {
        var path = Path.Combine(env.WebRootPath, "uploaded-files", folder, file.ToString());
        if (File.Exists(path)) File.Delete(path);

        var entity = db.Files.FirstOrDefault(f => f.Id == file);
        if (entity is not null) {
            db.Files.Remove(entity);
            var relations = db.FileRelations.Where(r => r.FileId == file);
            db.FileRelations.RemoveRange(relations);
            db.SaveChanges();
        }

        return true;

    }

    public async Task<Guid> UploadFilepondFile(IFormFile file, string folder, Guid userId) {
        if (file.Length <= 0) {
            return Guid.Empty;
        }

        var guid = Guid.CreateVersion7();

        var path = Path.Combine(env.WebRootPath, "uploaded-files", folder, guid.ToString());

        Directory.CreateDirectory(Path.Combine(env.WebRootPath, "uploaded-files", folder));

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(stream);

        await db.Files.AddAsync(new FileDbo {
            Id = guid,
            UserId = userId,
            FileName = file.FileName,
            MimeType = file.ContentType,
            Size = file.Length
        });
        await db.SaveChangesAsync();

        return guid;
    }

    public async Task LinkFile(Guid fileId, Guid entityId, string entityName) {
        await db.FileRelations.AddAsync(new FileRelationDbo {
            Id = Guid.CreateVersion7(),
            FileId = fileId,
            EntityId = entityId,
            EntityName = entityName
        });
        await db.SaveChangesAsync();
    }

    public void UnlinkFile(Guid fileId, Guid entityId) {
        var rel = db.FileRelations.FirstOrDefault(r => r.FileId == fileId && r.EntityId == entityId);
        if (rel is null) return;
        db.FileRelations.Remove(rel);
        db.SaveChanges();
    }

    public Task<FileDbo?> GetById(Guid id) {
        return db.Files.FirstOrDefaultAsync(f => f.Id == id);
    }
}