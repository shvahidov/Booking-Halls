using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Core.Entities
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; } // Название зала
        public ICollection<Booking> Bookings { get; set; }
    }


}
