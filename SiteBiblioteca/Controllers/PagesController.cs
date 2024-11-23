using Microsoft.AspNetCore.Mvc;
using SiteBiblioteca.Models;

namespace SiteBiblioteca.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult SobreNos(DadosBiblioteca _data)
        {
            _data = new DadosBiblioteca() { contactos = "teste", horario = "teste" };

            return View(_data);
        }

        public IActionResult RecuperarCodigoEmail()
        {
            return View();
        }
    }
}
