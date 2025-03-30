using Booking_Halls.Application.Interfaces;
using Booking_Halls.Core.Entities;
using Booking_Halls.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Booking_Halls.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        public BookingController(IBookingService bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var halls = await _bookingService.GetAllHallsAsync();
            return View(halls);
        }

        public async Task<IActionResult> HallDetails(int id)
        {
            var hall = await _bookingService.GetHallByIdAsync(id);
            if (hall == null)
            {
                return NotFound();
            }

            ViewBag.HallId = id;
            ViewBag.HallName = hall.Name; // Добавляем имя зала

            var bookings = await _bookingService.GetBookingsByHallAsync(id);

            return View(bookings);
        }


        public async Task<IActionResult> MyBookings()
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return Unauthorized();
            }

            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return View(bookings);
        }

        [HttpGet]
        public IActionResult Book(int hallId)
        {
            ViewBag.HallId = hallId;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Book(int hallId, DateTime startTime, DateTime endTime, string userName)
        {
            if (hallId <= 0)
            {
                ModelState.AddModelError("", "Неверный ID зала.");
                ViewBag.HallId = hallId;
                return View();
            }

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return Unauthorized();
            }

            
            // Проверка доступности перед бронированием
            string result = await _bookingService.BookHallAsync(hallId, userId, startTime, endTime, userName);
            if (result == "success")
            {
                return RedirectToAction("HallDetails", new { id = hallId });
            }

            ModelState.AddModelError("", result);
            ViewBag.HallId = hallId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return Unauthorized();
            }

            string result = await _bookingService.DeleteBookingAsync(bookingId, userId);
            if (result == "success")
            {
                return RedirectToAction("MyBookings");
            }

            ModelState.AddModelError("", result);
            return RedirectToAction("MyBookings");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAdminBooking(int bookingId)
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return Unauthorized();
            }

            string result = await _bookingService.DeleteAdminBookingAsync(bookingId);
            if (result == "success")
            {
                return RedirectToAction("MyBookings");
            }

            ModelState.AddModelError("", result);
            return RedirectToAction("MyBookings");
        }

    }
}
