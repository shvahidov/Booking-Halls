using Booking_Halls.Application.DTOs;
using Booking_Halls.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> RegisterUserAsync(RegisterRequestDto request);
    }
}
