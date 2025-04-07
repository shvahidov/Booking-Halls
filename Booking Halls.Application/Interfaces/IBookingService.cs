using Booking_Halls.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking_Halls.Application.Interfaces
{
    public interface IBookingService
    {
        Task<List<Hall>> GetAllHallsAsync();
        Task<List<Booking>> GetBookingsByHallAsync(int hallId);
        Task<bool> CanBookHallAsync(int hallId, DateTime startTime, DateTime endTime);
        Task<string> BookHallAsync(int hallId, int userId, DateTime startTime, DateTime endTime, string userName);
        Task<Hall> GetHallByIdAsync(int hallId);
        Task<List<Booking>> GetUserBookingsAsync(int userId);
        Task<string> DeleteBookingAsync(int bookingId, int userId);
        Task<string> DeleteAdminBookingAsync(int bookingId);
        Task RemoveExpiredBookingsAsync();

    }


}
