using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Application.DTOs
{
    public class LoginRequestDto
    {
        public required string Email { get; set; } = null!;
        public required string Password { get; set; } = null!;
    }
}
