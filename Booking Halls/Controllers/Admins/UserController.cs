using Booking_Halls.Application.Interfaces;
using Booking_Halls.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking_Halls.Controllers.Admins
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("admin/users")]
        [HttpGet]
        public async Task<IActionResult> ListUser()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [Route("admin/users/add")]
        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [Route("admin/users/add")]
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var newUser = await _userService.AddUserAsync(user);
            if (newUser == null)
            {
                ModelState.AddModelError("", "Не удалось добавить пользователя.");
                return View(user);
            }

            return RedirectToAction("ListUser");
        }

        [Route("admin/users/{id:int}/delete")]
        [HttpGet]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            return RedirectToAction("ListUser");
        }

    }
}
