using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using InertiaCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Yordanew.Models;

namespace Yordanew.Controllers;

public class HomeController(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager
) : Controller {
    [HttpGet("/")]
    public IActionResult Index() {
        return Inertia.Render("Index");
    }
    
    [HttpGet("/login")]
    public IActionResult Login() {
        return Inertia.Render("Login");
    }

    public class LoginRequest {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
    
    [HttpPost("/login")]
    public async Task<IActionResult> LoginPost([FromBody] LoginRequest request) {
        if (!ModelState.IsValid) {
            return Login();
        }
        try {
            var user = userManager.Users.First(u => u.UserName == request.UserName);
            
            var res = await signInManager.PasswordSignInAsync(user, request.Password, true, false);
            if (res.Succeeded) {
                return RedirectToAction("Index");
            }
        }
        catch (InvalidOperationException e) {}
        
        ModelState.AddModelError("Username", "Неправильный логин или пароль");
        
        return Inertia.Render("Login");
    }
    
    
    [HttpGet("/register")]
    public IActionResult Register() {
        return Inertia.Render("Register");
    }

    public class RegisterRequest {
        [Required(ErrorMessage = "Поле обязательно")]
        [MinLength(3, ErrorMessage = "Не меньше 3 символов")]
        public required string UserName { get; set; }
        
        [Required(ErrorMessage = "Поле обязательно")]
        public required string DisplayName { get; set; }
        
        [Required(ErrorMessage = "Поле обязательно")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты")]
        public required string Email { get; set; }
        
        [Required(ErrorMessage = "Поле обязательно")]
        [MinLength(8, ErrorMessage = "Не менее 8 символов")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Пароль должен содержать заглавные и строчные буквы, цифры и спецсимволы")]
        public required string Password { get; set; }
        
        [Required(ErrorMessage = "Поле обязательно")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public required string PasswordConfirm { get; set; }
    }
    
    [HttpPost("/register")]
    public async Task<IActionResult> RegisterPost([FromBody] RegisterRequest request) {
        if (!ModelState.IsValid) {
            return Register();
        }
        
        try {
            var user = new AppUser {
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName
            };
            var result = await userManager.CreateAsync(user, request.Password);
            
            if (result.Succeeded) {
                var res = await signInManager.PasswordSignInAsync(user, request.Password, true, false);
                if (res.Succeeded) {
                    return RedirectToAction("Index");
                }
            }
        }
        catch (InvalidOperationException e) {}
        
        ModelState.AddModelError("Password", "Пароль должен содержать заглавные и строчные буквы, цифры и спецсимволы");
        
        return Inertia.Render("Register");
    }
    
    [HttpGet("/logout")]
    public async Task<IActionResult> Logout() {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }
    
    
}