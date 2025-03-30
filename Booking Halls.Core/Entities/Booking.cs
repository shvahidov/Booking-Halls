using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int HallId { get; set; }
        public Hall Hall { get; set; } // Навигационное свойство

        public int UserId { get; set; }
        public User User { get; set; } // Навигационное свойство
        public string UserName { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }




}
