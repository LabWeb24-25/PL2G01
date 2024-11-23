using Microsoft.AspNetCore.Mvc;
using SiteBiblioteca.Data;
using SiteBiblioteca.Models;

namespace SiteBiblioteca.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public PagesController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult SobreNos()
        {
            // Busca um registro específico ou o primeiro registro da tabela
            var dadosBiblioteca = _context._dadosBiblioteca.FirstOrDefault();

            return View(dadosBiblioteca);
        }

        public IActionResult RecuperarCodigoEmail()
        {
            return View();
        }
    }
}
