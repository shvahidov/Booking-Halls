using Booking_Halls.Application.Interfaces;
using Booking_Halls.Infrastructure.Persistence;
using Booking_Halls.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
