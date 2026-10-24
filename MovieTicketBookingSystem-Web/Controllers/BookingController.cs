using Microsoft.AspNetCore.Mvc;
using MOVIETICKETBOOKINGSYSTEM.Models;
using MOVIETICKETBOOKINGSYSTEM.Services;

namespace MovieTicketBookingSystem_Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            // Load Customers
            List<Customer> customers = new();

            string customerPath = Path.Combine("Data", "customers.txt");

            if (System.IO.File.Exists(customerPath))
            {
                foreach (string line in System.IO.File.ReadAllLines(customerPath))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] data = line.Split('|');

                    customers.Add(
                        new Customer(
                            Convert.ToInt32(data[0]),
                            data[1],
                            Convert.ToDecimal(data[2])
                        )
                    );
                }
            }

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

                    Movie? movie =
                        movies.FirstOrDefault(m => m.Id == movieId);

                    Theater? theater =
                        theaters.FirstOrDefault(t => t.Id == theaterId);

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

            ViewBag.Customers = customers;
            ViewBag.Movies = movies;
            ViewBag.Shows = shows;

            return View();
        }


        [HttpPost]
        public IActionResult Book(
            string customerName,
            int showId,
            int ticketCount)
        {
            try
            {
        
                // Load Movies
                List<Movie> movies = new();

                string moviePath =
                    Path.Combine("Data", "movies.txt");

                foreach (string line in
                    System.IO.File.ReadAllLines(moviePath))
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


                // Load Theaters
                List<Theater> theaters = new();

                string theaterPath =
                    Path.Combine("Data", "theaters.txt");

                foreach (string line in
                    System.IO.File.ReadAllLines(theaterPath))
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


                // Load Shows
                List<Show> shows = new();

                string showPath =
                    Path.Combine("Data", "shows.txt");

                foreach (string line in
                    System.IO.File.ReadAllLines(showPath))
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] data = line.Split('|');

                    int movieId =
                        Convert.ToInt32(data[1]);

                    int theaterId =
                        Convert.ToInt32(data[2]);

                    Movie? movie =
                        movies.FirstOrDefault(
                            m => m.Id == movieId);

                    Theater? theater =
                        theaters.FirstOrDefault(
                            t => t.Id == theaterId);

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


                Show? selectedShow =
                    shows.FirstOrDefault(
                        s => s.Id == showId);

                if (selectedShow == null)
                {
                    TempData["Message"] =
                        "Show not found.";

                    return RedirectToAction("Index");
                }


                // Calculate Amount
                decimal amount =
                    selectedShow.Movie.TicketPrice *
                    ticketCount;

                if (string.IsNullOrWhiteSpace(customerName))
                {
                    TempData["Message"] = "Please enter your name.";
                    return RedirectToAction("Index");
                }

                Customer customer = new Customer(
                    0,
                    customerName.Trim(),
                    amount
                );



                // Create Booking
                Booking booking =
                    new Booking(
                        0,
                        customer,
                        selectedShow,
                        ticketCount,
                        amount
                    );


                // Process Booking
                _bookingService.BookTicket(booking);


                TempData["Success"] =
                    "Booking completed successfully!";

                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;

                return RedirectToAction("Index");
            }
        }


        public IActionResult Success()
        {
            return View();
        }
    }
}