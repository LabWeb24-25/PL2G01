using Microsoft.AspNetCore.Mvc;

namespace SiteBiblioteca.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult SobreNos()
        {
            return View();
        }
    }
}
