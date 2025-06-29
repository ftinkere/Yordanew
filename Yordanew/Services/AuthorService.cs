using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Yordanew.Domain.Entity;
using Yordanew.Models;

namespace Yordanew.Services;

public class AuthorService(UserManager<AppUser> userManager) {
    public Task<Author?> GetById(Guid id) {
        return userManager.Users
            .Where(u => u.Id == id)
            .Select(u => u.ToDomain())
            .FirstOrDefaultAsync();
    }

    public Task<Author?> GetByName(string userName) {
        return userManager.Users
            .Where(u => u.UserName == userName)
            .Select(u => u.ToDomain())
            .FirstOrDefaultAsync();
    }

    public async Task<Author?> Update(Author author) {
        var user = await userManager.FindByIdAsync(author.Id.ToString());
        if (user is null) return null;
        user.DisplayName = author.DisplayName;
        await userManager.UpdateAsync(user);
        return user.ToDomain();
    }
}
