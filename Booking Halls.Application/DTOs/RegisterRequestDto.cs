using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Application.DTOs
{
    public class RegisterRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public int Age { get; set; }
        public required string Role { get; set; }
    }
}
