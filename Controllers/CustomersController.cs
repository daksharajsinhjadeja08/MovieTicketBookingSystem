using Microsoft.AspNetCore.Mvc;
using MOVIETICKETBOOKINGSYSTEM.Models;

namespace MovieTicketBookingSystem_Web.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            List<Customer> customers = new();

            string path = Path.Combine("Data", "customers.txt");

            if (System.IO.File.Exists(path))
            {
                foreach (string line in System.IO.File.ReadAllLines(path))
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

            return View(customers);
        }
    }
}