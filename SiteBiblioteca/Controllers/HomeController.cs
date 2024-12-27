using Microsoft.AspNetCore.Identity;
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
            if(User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;

                var user = _context.Users.First(x => x.UserName == username);

                var adicional = _context.Adicional.First(x => x.Email == user.Email);

                ViewData["ProfileImage"] = adicional.image ?? "user.png"; // Usar uma imagem padrão se a imagem do perfil não estiver definida
            }

            HttpContext.Session.SetString("Source", "Index");

            // Obter os livros da base de dados com informações do autor
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
    }
}