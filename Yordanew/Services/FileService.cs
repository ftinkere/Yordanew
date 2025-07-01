namespace Yordanew.Services;

public class FileService(IWebHostEnvironment env) {
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
        if (!File.Exists(path)) return false;
        File.Delete(path);
        return true;

    }

    public async Task<Guid> UploadFilepondFile(IFormFile file, string folder) {
        if (file.Length <= 0) {
            return Guid.Empty;
        }

        var guid = Guid.CreateVersion7();
        
        var path = Path.Combine(env.WebRootPath, "uploaded-files", folder, guid.ToString());
        
        Directory.CreateDirectory(Path.Combine(env.WebRootPath, "uploaded-files", "dictionary"));

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(stream);
        
        return guid;
    }
}