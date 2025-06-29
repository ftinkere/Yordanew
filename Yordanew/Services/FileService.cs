namespace Yordanew.Services;

public class FileService(IWebHostEnvironment env) {
    public string GetAvatarPath(Guid userId) {
        return Path.Combine(env.WebRootPath, "uploaded-files", "avatars", userId.ToString());
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
}