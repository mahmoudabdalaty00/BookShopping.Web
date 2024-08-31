using BookShopping.Web.Models;
using BookShopping.Web.Models.DTOS;
using BookShopping.Web.Repositry;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShopping.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
 
            
            public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }


       

        public async Task<IActionResult> Index(string sterm ="" , int gendreId = 0)
        {
            IEnumerable<Book> books = await _homeRepository.GetBook(sterm,gendreId);
            IEnumerable<Gendre> genders = await _homeRepository.GetGenre();
            BookDisplayModel bookmodel = new BookDisplayModel()
            {
                Books = books,
                gendres = genders,
                Sterm = sterm,
                GendreID = gendreId,
            };

            return View(bookmodel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
