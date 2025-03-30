using Booking_Halls.Application.DTOs;
using FluentValidation;

namespace Booking_Halls.Application.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Имя пользователя обязательно")
                .MinimumLength(3).WithMessage("Имя пользователя должно содержать минимум 3 символа")
                .MaximumLength(20).WithMessage("Имя пользователя не должно превышать 20 символов");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов")
                .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
                .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат email");

            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(18).WithMessage("Возраст должен быть не менее 18 лет");

            
        }
    }
}
