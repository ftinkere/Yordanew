using InertiaCore;
using InertiaCore.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Yordanew;
using Yordanew.Models;
using Yordanew.Models;
using Yordanew.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options
    .UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

builder.Services.AddInertia(options => {
    options.RootView = "~/Views/App.cshtml";
});
builder.Services.AddViteHelper(options => {
    options.PublicDirectory = "wwwroot";
    options.BuildDirectory = "build";
    options.HotFile = "hot";
    options.ManifestFilename = "manifest.json";
});

builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<LanguageService>();
builder.Services.AddScoped<DictionaryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
}
else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions {
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});
app.UseRouting();

app.UseInertia();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) => {
    var username = context.User.Identity?.Name;
    var manager = context.RequestServices.GetRequiredService<UserManager<AppUser>>();
    try {
        var user = manager.Users.First(u => u.UserName == username);
        Inertia.Share("auth", new {
            User = new {
                Id = user.Id,
                Username = user.UserName,
                DisplayName = user.DisplayName,
            }
        });
    } catch(InvalidOperationException e) {} finally {
        await next();
    }
});

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();