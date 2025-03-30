using Booking_Halls.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Booking_Halls.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
