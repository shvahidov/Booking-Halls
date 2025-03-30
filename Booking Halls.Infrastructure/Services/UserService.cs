using AutoMapper;
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
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationContext _context;
        public UserService(IRepository<User> userRepository, IMapper mapper, ApplicationContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
        }
        public async Task<User> AddUserAsync(User user)
        {
            user.Password = PasswordHelper.HashPassword(user.Password);
            await _userRepository.AddAsync(user);
            Console.WriteLine($"Полученная роль: {user.Role}");
            await _userRepository.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)  // Возвращаем bool
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _userRepository.Delete(user);
                await _userRepository.SaveChangesAsync();
                return true;  // Успешное удаление
            }
            return false;  // Если пользователь не найден
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<bool> RegisterUserAsync(RegisterRequestDto request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return false; // Пользователь уже существует
            }

            var user = _mapper.Map<User>(request);
            user.Password = PasswordHelper.HashPassword(request.Password);
            user.Role = "User"; // Присваиваем роль "User"

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
