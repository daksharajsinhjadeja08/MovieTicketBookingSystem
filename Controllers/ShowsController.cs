using Microsoft.AspNetCore.Mvc;
using MOVIETICKETBOOKINGSYSTEM.Models;

namespace MovieTicketBookingSystem_Web.Controllers
{
    public class ShowsController : Controller
    {
        public IActionResult Index()
        {
            // Load Movies
            List<Movie> movies = new();

            string moviePath = Path.Combine("Data", "movies.txt");

            if (System.IO.File.Exists(moviePath))
            {
                foreach (string line in System.IO.File.ReadAllLines(moviePath))
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

            // Load Theaters
            List<Theater> theaters = new();

            string theaterPath = Path.Combine("Data", "theaters.txt");

            if (System.IO.File.Exists(theaterPath))
            {
                foreach (string line in System.IO.File.ReadAllLines(theaterPath))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] data = line.Split('|');

                    theaters.Add(
                        new Theater(
                            Convert.ToInt32(data[0]),
                            data[1],
                            data[2]
                        )
                    );
                }
            }

            // Load Shows
            List<Show> shows = new();

            string showPath = Path.Combine("Data", "shows.txt");

            if (System.IO.File.Exists(showPath))
            {
                foreach (string line in System.IO.File.ReadAllLines(showPath))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] data = line.Split('|');

                    int movieId = Convert.ToInt32(data[1]);
                    int theaterId = Convert.ToInt32(data[2]);

                    Movie? movie = movies.FirstOrDefault(m => m.Id == movieId);
                    Theater? theater = theaters.FirstOrDefault(t => t.Id == theaterId);

                    if (movie != null && theater != null)
                    {
                        shows.Add(
                            new Show(
                                Convert.ToInt32(data[0]),
                                movie,
                                theater,
                                data[3]
                            )
                        );
                    }
                }
            }

            return View(shows);
        }
    }
}