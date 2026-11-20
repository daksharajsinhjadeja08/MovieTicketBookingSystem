using Microsoft.AspNetCore.Mvc;
using MOVIETICKETBOOKINGSYSTEM.Models;

namespace MovieTicketBookingSystem_Web.Controllers
{
    public class MoviesController : Controller
    {
        public IActionResult Index()
        {
            List<Movie> movies = new();

            string path = Path.Combine("Data", "movies.txt");

            if (System.IO.File.Exists(path))
            {
                foreach (string line in System.IO.File.ReadAllLines(path))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] data = line.Split('|');

                    movies.Add(
                        new Movie(
                            Convert.ToInt32(data[0]),
                            data[1],
                            data[2],
                            Convert.ToInt32(data[3]),
                            Convert.ToDecimal(data[4])
                        )
                    );
                }
            }

            return View(movies);
        }
    }
}