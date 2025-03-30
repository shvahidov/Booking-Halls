using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Age { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }

        public List<Booking> Bookings { get; set; } = new(); // Связь с бронированиями
    }

}
