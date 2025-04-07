using Booking_Halls.Application.Interfaces;
using Booking_Halls.Core.Entities;
using Booking_Halls.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Booking_Halls.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationContext _context;

        public BookingService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Hall>> GetAllHallsAsync()
        {
            return await _context.Halls.ToListAsync();
        }

        public async Task<List<Booking>> GetBookingsByHallAsync(int hallId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.HallId == hallId)
                .ToListAsync();

            return bookings;
        }

        public async Task<bool> CanBookHallAsync(int hallId, DateTime startTime, DateTime endTime)
        {
            DateTime startUtc = startTime.ToUniversalTime();
            DateTime endUtc = endTime.ToUniversalTime();

            // Проверяем, занят ли Переговорный зал (ID 4), если бронируется PS-зал (ID 3)
            if (hallId == 3)
            {
                bool isMeetingRoomBooked = await _context.Bookings
                    .AnyAsync(b => b.HallId == 4 &&
                                   ((startUtc >= b.StartTime && startUtc < b.EndTime) ||
                                    (endUtc > b.StartTime && endUtc <= b.EndTime)));

                if (isMeetingRoomBooked) return false;
            }

            // Проверяем, есть ли пересечение бронирований
            bool isAvailable = !await _context.Bookings
                .AnyAsync(b => b.HallId == hallId &&
                               ((startUtc >= b.StartTime && startUtc < b.EndTime) ||
                                (endUtc > b.StartTime && endUtc <= b.EndTime) ||
                                (startUtc <= b.StartTime && endUtc >= b.EndTime))); // Полное покрытие

            return isAvailable;
        }

        public async Task<string> BookHallAsync(int hallId, int userId, DateTime startTime, DateTime endTime, string userName)
        {
            var hallExists = await _context.Halls.AnyAsync(h => h.Id == hallId);
            if (!hallExists)
            {
                return "Зал не найден."; // Зала нет - нельзя бронировать
            }

            DateTime startUtc = startTime.ToUniversalTime();
            DateTime endUtc = endTime.ToUniversalTime();

            // 🔹 Проверка на конфликт бронирования в этом зале
            bool isHallAvailable = !await _context.Bookings
                .AnyAsync(b => b.HallId == hallId &&
                               ((startUtc >= b.StartTime && startUtc < b.EndTime) ||
                                (endUtc > b.StartTime && endUtc <= b.EndTime)));

            if (!isHallAvailable)
            {
                return "Зал уже забронирован на выбранное время.";
            }

            // 🔹 Проверка PS-зала (ID: 3) и Переговорного зала (ID: 4)
            if (hallId == 3) // Если бронируется PS-зал
            {
                bool isMeetingRoomBooked = await _context.Bookings
                    .AnyAsync(b => b.HallId == 4 &&
                                   ((startUtc >= b.StartTime && startUtc < b.EndTime) ||
                                    (endUtc > b.StartTime && endUtc <= b.EndTime)));

                if (isMeetingRoomBooked)
                {
                    return "PS-зал недоступен, так как в Переговорном зале уже есть бронь.";
                }
            }

            var booking = new Booking
            {
                HallId = hallId,
                UserId = userId,
                StartTime = startUtc,
                EndTime = endUtc,
                UserName = userName
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return "success"; // Успешное бронирование
        }


        public async Task<Hall> GetHallByIdAsync(int hallId)
        {
            return await _context.Halls.FindAsync(hallId);
        }

        public async Task<List<Booking>> GetUserBookingsAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Hall)
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }

        //Удаляет просроченные бронирования, у которых EndTime уже прошло.
        public async Task RemoveExpiredBookingsAsync()
        {
            var now = DateTime.UtcNow;
            var expiredBookings = await _context.Bookings
                .Where(b => b.EndTime < now)
                .ToListAsync();

            if (expiredBookings.Any())
            {
                _context.Bookings.RemoveRange(expiredBookings);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> DeleteBookingAsync(int bookingId, int userId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return "Бронирование не найдено.";
            }

            if (booking.UserId != userId)
            {
                return "Вы не можете удалить чужую бронь.";
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return "success";
        }

        public async Task<string> DeleteAdminBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return "Бронирование не найдено.";
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return "success";
        }
    }
}
