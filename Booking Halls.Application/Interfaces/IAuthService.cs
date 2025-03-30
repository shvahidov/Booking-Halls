using Booking_Halls.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> AuthenticateAsync(LoginRequestDto request);
        Task EnsureAdminExistsAsync();
    }
}
