using Microsoft.AspNetCore.Mvc;

namespace SiteBiblioteca.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult EditarPerfil()
        {
            return View();
        }
    }
}
