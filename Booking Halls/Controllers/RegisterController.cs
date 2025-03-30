using Booking_Halls.Application.DTOs;
using Booking_Halls.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Halls.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegisterRequestDto> _validator;

        public RegisterController(IUserService userService, IValidator<RegisterRequestDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(request);
            }

            if (!await _userService.RegisterUserAsync(request))
            {
                ModelState.AddModelError("", "Такой пользователь уже существует");
                return View(request);
            }

            return RedirectToAction("Login", "Login");
        }
    }
}
