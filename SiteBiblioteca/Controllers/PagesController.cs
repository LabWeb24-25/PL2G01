using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var dadosBiblioteca = _context._dadosBiblioteca.FirstOrDefault(); //buscar registo da primeira linha da tabela dadosBiblioteca

            return View(dadosBiblioteca);
        }

        [Authorize(Roles = "Leitor")]
        public IActionResult RecuperarCodigoEmail()
        {
            return View();
        }

        [Authorize(Roles = "Leitor")]
        public IActionResult SobreLivro(string ISBN)
        {
            var livro = _context.livros
                .Include(l => l.autor)
                .FirstOrDefault(x => x.ISBN == ISBN);

            return View(livro);
        }

        [Authorize(Roles = "Bibliotecário")]
        public IActionResult AdicionarLivro()
        {
            return View();
        }

        [Authorize(Roles = "Bibliotecário")]
        public IActionResult EditarLivro()
        {
            return View();
        }
    }
}
