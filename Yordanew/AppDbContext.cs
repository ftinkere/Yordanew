using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yordanew.Models;
using Yordanew.Models;

namespace Yordanew;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options) {
    public DbSet<LanguageDbo> Languages { get; set; }
    public DbSet<ArticleDbo> Articles { get; set; }
    public DbSet<LexemeDbo> Lexemes { get; set; }
    public DbSet<FileDbo> Files { get; set; }
    public DbSet<FileRelationDbo> FileRelations { get; set; }
}