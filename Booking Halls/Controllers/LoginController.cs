using Booking_Halls.Application.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Booking_Halls.Application.Interfaces;

namespace Booking_Halls.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await _authService.EnsureAdminExistsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            

            var userDto = await _authService.AuthenticateAsync(request);

            if (userDto == null)
            {
                ModelState.AddModelError("", "Неверный email или пароль");
                return View(request);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Name, userDto.Email),
                new Claim(ClaimTypes.Role, userDto.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return userDto.Role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "User" => RedirectToAction("Index", "Booking"),
                _ => RedirectToAction("Login", "Login")
            };
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

