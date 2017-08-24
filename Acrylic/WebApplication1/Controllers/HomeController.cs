using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Acrylic.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private const int MaxPrimeNumber = 1024;
        private PrimeNumberProvider primeNumberProvider;
        /// <summary>
        /// Injection constructor used to demonstrate an MVC controller dependent on a
        /// service.  The <paramref name="primeNumberProvider"/> really has no significance in the
        /// exercise other than to demonstrate that the dependency injection.  
        /// The controller is dependent on a concrete type but that is intentional as well, it is not material 
        /// to the demonstration that IOC container injecting a dependency required by the controller.
        /// </summary>
        /// <param name="primeNumberProvider">Service use to calculate prime numbers.</param>
        public HomeController(PrimeNumberProvider primeNumberProvider) {

            this.primeNumberProvider = primeNumberProvider;
        }
        public IActionResult Index()
        {
            //The primeNumberProvider service dependency used
            var primes = primeNumberProvider.GetPrimes(MaxPrimeNumber);
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
