using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Booking_Halls.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //// Регистрируем AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Регистрируем все валидаторы FluentValidation из текущей сборки
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
