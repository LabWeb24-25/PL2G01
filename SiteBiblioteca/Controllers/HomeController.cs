using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteBiblioteca.Data;
using SiteBiblioteca.Models;
using System.Diagnostics;

namespace SiteBiblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var livros = _context.livros.Include(l => l.autor).ToList();
            return View(livros);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult VerRequisicoes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Confirmar()
        {
            // Lógica para confirmar solicitação
            return RedirectToAction("NotificacoesAdministrador");
        }

        [HttpPost]
        public IActionResult Negar()
        {
            // Lógica para negar solicitação
            return RedirectToAction("NotificacoesAdministrador");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
