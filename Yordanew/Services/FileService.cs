namespace Yordanew.Services;

public class FileService {
    public static string GetAvatarPath(Guid userId) {
        return Path.Combine("uploaded-files", "avatars", userId.ToString());
    }

    public bool UploadAvatar(IFormFile file, Guid userId) {
        if (file.Length <= 0 || file.ContentType.StartsWith("image/")) {
            return false;
        }

        var path = GetAvatarPath(userId);
        Directory.CreateDirectory(Path.Combine("uploaded-files", "avatars"));

        using var stream = new FileStream(path, FileMode.Create);
        file.CopyTo(stream);

        return true;
    }
}