using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private Acrylic.Services.PrimeNumberProvider primeNumberProvider;

        public HomeController() {
            primeNumberProvider = new Acrylic.Services.PrimeNumberProvider();
        }
        public IActionResult Index()
        {
            var primes = primeNumberProvider.GetPrimes(1024);
            ViewData["Primes"] = primes;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
