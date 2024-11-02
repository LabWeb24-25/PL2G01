using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteBiblioteca.Models;
using System.Diagnostics;

namespace SiteBiblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Nova a��o para redirecionar para a p�gina de perfil
        public IActionResult Perfil()
        {
            // Redireciona para a p�gina de perfil localizada na �rea "Identity"
            return RedirectToPage("/Account/Manage/Perfil", new { area = "Identity" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
