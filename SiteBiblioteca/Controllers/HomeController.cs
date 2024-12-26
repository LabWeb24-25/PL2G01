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
            // Obter os livros da base de dados com informa��es do autor
            var livros = _context.livros.Include(l => l.autor).ToList();
            return View(livros);
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

        [HttpGet]
        public JsonResult ObterAtrasos()
        {
            var atrasos = _context.requisicoes
                .Include(r => r.leitor)
                .Include(r => r.livro)
                    .ThenInclude(r => r.autor)
                .Where(x => x.data_entrega < DateTime.Now && x.biblioRecebeId != null)
                .ToList();

            return Json(new { count = atrasos.Count() });
        }

        [HttpGet]
        public JsonResult ObterAprovacoes()
        {
            var aprovacoes = _context.Adicional.Where(x => x.confirmado == false);

            return Json(new { count = aprovacoes.Count() });
        }
    }
}