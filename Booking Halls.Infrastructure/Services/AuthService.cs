using Booking_Halls.Application.DTOs;
using Booking_Halls.Application.Interfaces;
using Booking_Halls.Core.Entities;
using Booking_Halls.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationContext _context;

        public AuthService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task EnsureAdminExistsAsync()
        {
            if (!_context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    Name = "Admin",
                    Age = 40,
                    Email = "admin@example.com",
                    Password = PasswordHelper.HashPassword("Admin123!"),
                    Role = "Admin"
                };

                _context.Users.Add(admin);
                _context.SaveChanges();
            }
        }

        public async Task<UserDto?> AuthenticateAsync(LoginRequestDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.Password))
                return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
