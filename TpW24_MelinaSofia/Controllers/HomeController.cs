using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TpW24_MelinaSofia.Models;
using TpW24_MelinaSofia.ViewModels;

namespace TpW24_MelinaSofia.Controllers
{
    public class HomeController : Controller
    {

        private readonly ForumSofiaMelinaContext _context;
      
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ForumSofiaMelinaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var homeCats = _context.Categories.Where(c=>c.Actif == true).Select(c=> new 
            HomeCat
            {
                CatId = c.CatId,
                Nom = c.Nom,
                Description = c.Description,
                Actif = c.Actif,
                TotalSujets = c.Sujets.Count
            });

            return View(homeCats);
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