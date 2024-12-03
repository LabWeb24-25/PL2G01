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

        public IActionResult PainelAdministrador()
        {
            // Busca a lista de usuários do banco de dados
            var utilizador = _context.Adicional.ToList();
            return View(utilizador); // Passa a lista para a view
        }

        [HttpPost]
        //public IActionResult Banir(int id)
        //{
        //    var utilizador = _context.Adicional.Find(id);
        //    if (utilizador != null)
        //    {
        //        // Marcar o usuário como banido
        //        utilizador.banido = true;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("PainelAdministrador");
        //}

        //[HttpPost]
        //public IActionResult Desbanir(int id)
        //{
        //    var utilizador = _context.Adicional.Find(id);
        //    if (utilizador != null)
        //    {
        //        // Marcar o usuário como não banido
        //        utilizador.banido = false;
        //        _context.SaveChanges();
        //    }
        //    return RedirectToAction("PainelAdministrador");
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
