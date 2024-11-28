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

            // Verifica e corrige o link do YouTube
            if (!string.IsNullOrEmpty(dadosBiblioteca.youtube))
            {
                if (!dadosBiblioteca.youtube.StartsWith("https://"))
                {
                    dadosBiblioteca.youtube = "https://" + dadosBiblioteca.youtube;
                }
                if (dadosBiblioteca.youtube.StartsWith("https://www.") == false && dadosBiblioteca.youtube.Contains("www."))
                {
                    dadosBiblioteca.youtube = dadosBiblioteca.youtube.Replace("www.", "https://www.");
                }
            }

            // Verifica e corrige o link do X (Twitter)
            if (!string.IsNullOrEmpty(dadosBiblioteca.x))
            {
                if (!dadosBiblioteca.x.StartsWith("https://"))
                {
                    dadosBiblioteca.x = "https://" + dadosBiblioteca.x;
                }
                if (dadosBiblioteca.x.StartsWith("https://www.") == false && dadosBiblioteca.x.Contains("www."))
                {
                    dadosBiblioteca.x = dadosBiblioteca.x.Replace("www.", "https://www.");
                }
            }

            // Verifica e corrige o link do Instagram
            if (!string.IsNullOrEmpty(dadosBiblioteca.instagram))
            {
                if (!dadosBiblioteca.instagram.StartsWith("https://"))
                {
                    dadosBiblioteca.instagram = "https://" + dadosBiblioteca.instagram;
                }
                if (dadosBiblioteca.instagram.StartsWith("https://www.") == false && dadosBiblioteca.instagram.Contains("www."))
                {
                    dadosBiblioteca.instagram = dadosBiblioteca.instagram.Replace("www.", "https://www.");
                }
            }

            // Verifica e corrige o link do TikTok
            if (!string.IsNullOrEmpty(dadosBiblioteca.tiktok))
            {
                if (!dadosBiblioteca.tiktok.StartsWith("https://"))
                {
                    dadosBiblioteca.tiktok = "https://" + dadosBiblioteca.tiktok;
                }
                if (dadosBiblioteca.tiktok.StartsWith("https://www.") == false && dadosBiblioteca.tiktok.Contains("www."))
                {
                    dadosBiblioteca.tiktok = dadosBiblioteca.tiktok.Replace("www.", "https://www.");
                }
            }

            // Verifica e corrige o link do Facebook
            if (!string.IsNullOrEmpty(dadosBiblioteca.facebook))
            {
                if (!dadosBiblioteca.facebook.StartsWith("https://"))
                {
                    dadosBiblioteca.facebook = "https://" + dadosBiblioteca.facebook;
                }
                if (dadosBiblioteca.facebook.StartsWith("https://www.") == false && dadosBiblioteca.facebook.Contains("www."))
                {
                    dadosBiblioteca.facebook = dadosBiblioteca.facebook.Replace("www.", "https://www.");
                }
            }

            return View(dadosBiblioteca);
        }

        //[Authorize(Roles = "Leitor")]
        public IActionResult RecuperarCodigoEmail()
        {
            return View();
        }

        //[Authorize(Roles = "Leitor")]
        public IActionResult SobreLivro(string ISBN)
        {
            var livro = _context.livros
                .Include(l => l.autor)
                .FirstOrDefault(x => x.ISBN == ISBN);

            return View(livro);
        }

        //[Authorize(Roles = "Bibliotecário")]
        public IActionResult AdicionarLivro()
        {
            return View();
        }

        //[Authorize(Roles = "Bibliotecário")]
        public IActionResult EditarLivro(string ISBN)
        {
            var livro = _context.livros
                .Include(l => l.autor)
                .FirstOrDefault(x => x.ISBN == ISBN);
            return View(livro);
        }

        [HttpPost]
        public IActionResult GuardarEdicao(Livro model)
        {
            if (ModelState.IsValid)
            {
                var livroExistente = _context.livros.FirstOrDefault(l => l.ISBN == model.ISBN);

                if (livroExistente != null)
                {
                    // Atualizar os dados do livro existente
                    livroExistente.titulo = model.titulo;
                    livroExistente.autor = model.autor;
                    livroExistente.genero = model.genero;
                    livroExistente.preco = model.preco;
                    livroExistente.numExemplares = model.numExemplares;

                    _context.SaveChanges();
                }
                else
                {
                    // Caso não exista, criar um novo livro
                    _context.livros.Add(model);
                    _context.SaveChanges();
                }

                return RedirectToAction("Index"); // Redirecionar para uma página específica
            }

            return View(model); // Retornar à mesma página em caso de erro
        }

        //[Authorize(Roles = "Leitor")]
        public IActionResult Pesquisa(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
            {
                // Obtém a URL da página anterior
                var referer = Request.Headers["Referer"].ToString();

                // Se a URL de origem for válida, redireciona para lá
                if (!string.IsNullOrEmpty(referer))
                {
                    return Redirect(referer);
                }

                // Caso não tenha referer, redireciona para a página inicial
                return RedirectToAction("Index");
            }

            var livros = _context.livros
                .Where(l => l.titulo.Contains(termo) || l.autor.Nome.Contains(termo) || l.genero.Contains(termo))
                .Include(l => l.autor)
                .ToList();

            return View(livros);
        }

        public IActionResult SobreAutor(int id)
        {
            var autor = _context.autores.FirstOrDefault(l => l.Id == id);

            return View(autor);
        }
    }
}
