using Microsoft.AspNetCore.Mvc;

namespace SiteBiblioteca.Controllers
{
    public class HomeController2 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EditarPerfil2()
        {
            return View();
        }
    }
}
