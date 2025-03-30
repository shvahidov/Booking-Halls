using Booking_Halls.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Halls.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;

        public AdminController(IBookingService bookingService, IUserService userService)
        {
            _bookingService = bookingService;
            _userService = userService;
        }
        public async Task<IActionResult> DashboardAsync()
        {
            var halls = await _bookingService.GetAllHallsAsync();
            return View(halls);
        }
    }
}
