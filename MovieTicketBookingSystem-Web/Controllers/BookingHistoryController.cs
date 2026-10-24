using Microsoft.AspNetCore.Mvc;

namespace MovieTicketBookingSystem_Web.Controllers
{
    public class BookingHistoryController : Controller
    {
        public IActionResult Index()
        {
            string path = Path.Combine(
                "Bookings",
                "bookings.txt");

            List<string> bookings = new();

            if (System.IO.File.Exists(path))
            {
                string content =
                    System.IO.File.ReadAllText(path);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    bookings = content
                        .Split("--------------------------------")
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();
                }
            }

            return View(bookings);
        }
    }
}