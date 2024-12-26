using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
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
        private readonly SignInManager<IdentityUser> _signInManager;

        public PagesController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Desconectar o utilizador
            return RedirectToAction("Index", "Home"); // Redirecionar para a página inicial
        }

        public IActionResult SobreNos()
        {
            var dadosBiblioteca = _context._dadosBiblioteca.FirstOrDefault(); // buscar registo da primeira linha da tabela dadosBiblioteca

            // Função para corrigir qualquer link e garantir o formato adequado
            string CorrigirLink(string link)
            {
                if (string.IsNullOrEmpty(link)) return link;

                // Verifica se o link já começa com 'https://'
                if (!link.StartsWith("https://"))
                {
                    link = "https://" + link;
                }

                // Verifica e ajusta o domínio www. caso necessário
                if (!link.StartsWith("https://www.") && link.Contains("www."))
                {
                    link = link.Replace("www.", "https://www.");
                }

                return link;
            }

            if(dadosBiblioteca != null)
            {
                // Verifica e corrige o link do YouTube
                dadosBiblioteca.youtube = CorrigirLink(dadosBiblioteca.youtube);

                // Verifica e corrige o link do X (Twitter)
                dadosBiblioteca.x = CorrigirLink(dadosBiblioteca.x);

                // Verifica e corrige o link do Instagram
                dadosBiblioteca.instagram = CorrigirLink(dadosBiblioteca.instagram);

                // Verifica e corrige o link do TikTok
                dadosBiblioteca.tiktok = CorrigirLink(dadosBiblioteca.tiktok);

                // Verifica e corrige o link do Facebook
                dadosBiblioteca.facebook = CorrigirLink(dadosBiblioteca.facebook);
            }

            return View(dadosBiblioteca);
        }

        public IActionResult RecuperarCodigoEmail()
        {
            return View();
        }

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
            var autores = _context.autores.ToList();

            return View(autores);
        }

        [HttpPost]
        public async Task<IActionResult> adicionarLivro(Livro livro, IFormFile Capa)
        {
            if (ModelState.IsValid)
            {
                // Processar upload da imagem, se necessário
                if (Capa != null && Capa.Length > 0)
                {
                    var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    var nomeFicheiro = Guid.NewGuid().ToString() + Path.GetExtension(Capa.FileName);

                    var caminhoCompleto = Path.Combine(caminhoPasta, nomeFicheiro);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        await Capa.CopyToAsync(stream);
                    }

                    // Armazenar o caminho no banco de dados
                    livro.imagem = "/img/" + nomeFicheiro;
                }

                // Salvar no banco de dados
                _context.livros.Add(livro);
                await _context.SaveChangesAsync();

                // Redirecionar após sucesso
                return RedirectToAction("Index");
            }

            // Caso os dados não sejam válidos, retornar o formulário com os erros
            return View(livro);
        }

        [Authorize(Roles = "Bibliotecário")]
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
            var autor = _context.autores.First(l => l.Id == id);

            return View(autor);
        }

        [Authorize(Roles = "Bibliotecário")]
        public IActionResult AdicionarAutor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> adicionarAutorNovo(Autor autor, IFormFile Imagem)
        {
            if (ModelState.IsValid)
            {
                if (Imagem != null && Imagem.Length > 0)
                {
                    var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                    var nomeFicheiro = Guid.NewGuid().ToString() + Path.GetExtension(Imagem.FileName);

                    var caminhoCompleto = Path.Combine(caminhoPasta, nomeFicheiro);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        await Imagem.CopyToAsync(stream);
                    }

                    autor.Imagem = "/img/" + nomeFicheiro;
                }

                _context.autores.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(autor);
        }

        //[Authorize(Roles = "Administrador")]
        public IActionResult EditarDadosBiblioteca()
        {
            var dados = _context._dadosBiblioteca.FirstOrDefault(); // Busca a única linha da base de dados na tabela _dadosBiblioteca

            return View(dados);
        }

        //[Authorize(Roles = "Administrador")]
        public IActionResult PainelAdministrador()
        {
            var utilizador = _context.Adicional.ToList(); // Busca a lista de utilizadores do banco de dados

            return View(utilizador); // Passa a lista para a view
        }

        [HttpPost]
        public IActionResult Banir(int userId, string reason)
        {
            var adicional = _context.Adicional.First(x => x.Id == userId);

            // Marcar o utilizador como banido
            adicional.banido = true;
            _context.SaveChanges();

            var admin = User.Identity.Name;

            var adminAspNetUsers = _context.Users.First(x => x.UserName == admin);

            var adminAdicional = _context.Adicional.First(x => x.Email == adminAspNetUsers.Email);

            _context.bloqueios.Add(new Bloqueio
            {
                userId = adicional.Id,
                adminId = adminAdicional.Id,
                motivo = reason,
                dataBloqueio = DateTime.Now
            });

            _context.SaveChanges();

            return Redirect("/Pages/PainelAdministrador");
        }

        [HttpPost]
        public IActionResult Desbanir(int id)
        {
            var utilizador = _context.Adicional.First(x => x.Id == id);

            utilizador.banido = false;
            _context.SaveChanges();

            var desbanido = _context.bloqueios.First(x => x.userId == id);

            _context.bloqueios.Remove(desbanido);
            _context.SaveChanges();

            return Redirect("/Pages/PainelAdministrador");
        }

        //[Authorize(Roles = "Bibliotecário")]
        public IActionResult PainelBibliotecario(string? termo)
        {
            var livros = new List<Livro>();

            if (string.IsNullOrWhiteSpace(termo))
            {
                livros = _context.livros.Include(l => l.autor).ToList();
            }
            else
            {
                livros = _context.livros
                            .Where(l => l.titulo.Contains(termo) || l.autor.Nome.Contains(termo) || l.genero.Contains(termo))
                            .Include(l => l.autor)
                            .ToList();
            }

            return View(livros);
        }


        public IActionResult Bloquear(int id)
        {
            var utilizador = _context.Adicional.First(x => x.Id == id);

            return View(utilizador);
        }

        //[Authorize(Roles = "Administrador")]
        public IActionResult NotificacoesAdministrador()
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

        public IActionResult EditarPerfil()
        {
            var username = User.Identity.Name; // obtenção do username do utilizador

            var useraspnet = _context.Users.First(x => x.UserName == username); // Linha correspondente ao AspNetUsers

            var adicional = _context.Adicional.First(x => x.Email == useraspnet.Email); // Linha correspondente ao Adicional

            return View(adicional);
        }

        [HttpPost]
        public async Task<IActionResult> guardarPerfil(string novousername, string nome, string email, string morada, string contactos)
        {
            var username = User.Identity.Name; // Obter o username do utilizador autenticado

            // Verificar se o utilizador existe na tabela AspNetUsers
            var useraspnet = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (useraspnet == null)
            {
                return NotFound("Utilizador não encontrado.");
            }

            // Obter os dados adicionais do utilizador na tabela Adicional
            var adicional = await _context.Adicional.FirstOrDefaultAsync(x => x.Email == useraspnet.Email);
            if (adicional == null)
            {
                return NotFound("Informações adicionais não encontradas.");
            }

            // Atualizar os dados no AspNetUsers
            useraspnet.UserName = novousername;
            await _context.SaveChangesAsync();

            // Atualizar os dados no Adicional
            adicional.Name = nome; // Nome
            adicional.Address = morada; // Morada
            adicional.Contact = contactos; // Contactos
            await _context.SaveChangesAsync(); // Salvar mudanças na tabela Adicional

            // Redirecionar para a página PersonalData
            return Redirect("/Identity/Account/Manage/PersonalData");
        }


        public IActionResult EmailConfirmado()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Criar_Administrador()
        { 
            return View();
        }

        public IActionResult Alterar_Codigo_Acesso()
        {
            return View();
        }

        public IActionResult UtilizadorBloqueado()
        {
            var username = User.Identity.Name; // obtenção do username do utilizador

            var useraspnet = _context.Users.First(x => x.UserName == username); // Linha correspondente ao AspNetUsers

            var idAdicional = _context.Adicional.First(x => x.Email == useraspnet.Email); // Linha correspondente ao Adicional

            var bloqueio = _context.bloqueios.First(x => x.userId == idAdicional.Id); // Linha correspondente ao bloqueios

            if(bloqueio != null)
            {
                _signInManager.SignOutAsync(); // Desconectar o utilizador para não aceder à conta por URL
                return View(bloqueio);
            }

            return RedirectToPage("Home/Index");
        }

        //[Authorize(Roles = "Bibliotecário")]
        public async Task<IActionResult> VerRequisicoes()
        {
            var requisicoes = await _context.requisicoes
                .Include(r => r.leitor)
                .Include(r => r.livro)
                    .ThenInclude(r => r.autor)
                .Where(x => ((x.data_entrega > DateTime.Now && x.biblioRecebeId == null) || x.biblioEntregaId == null))
                .ToListAsync();

            return View(requisicoes);
        }

        //[Authorize(Roles = "Bibliotecário")]
        public async Task<IActionResult> NotificacoesBibliotecario()
        {
            var requisicoes = await _context.requisicoes
                                .Include(r => r.leitor)
                                .Include(r => r.livro)
                                    .ThenInclude(r => r.autor)
                                .Where(x => (x.data_entrega < DateTime.Now && x.biblioRecebeId != null))
                                .ToListAsync();

            return View(requisicoes);
        }

        [HttpPost]
        public IActionResult PedirRequisicao(string ISBN)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                TempData["Mensagem"] = "Precisa de estar logado para requisitar um livro.";

                return Redirect("SobreLivro?ISBN=" + ISBN);
            }

            var username = User.Identity.Name; // obtenção do username do utilizador

            var useraspnet = _context.Users.First(x => x.UserName == username); // Linha correspondente ao AspNetUsers

            var idAdicional = _context.Adicional.First(x => x.Email == useraspnet.Email); // Linha correspondente ao Adicional

            var requisicao = new Requisitar()
            {
                leitorId = idAdicional.Id,
                data_requisicao = DateTime.Now,
                data_entrega = DateTime.Now.AddDays(30),
                livroISBN = ISBN,
            };

            var repetido = _context.requisicoes.FirstOrDefault(x => x.leitorId == idAdicional.Id && x.livroISBN == ISBN);

            if (repetido != null)
            {
                TempData["Mensagem"] = "Pedido de requisição já efetuado.";

                return Redirect("SobreLivro?ISBN=" + ISBN);
            }

            _context.requisicoes.Add(requisicao);
            _context.SaveChanges();

            TempData["Mensagem"] = "Pedido de requisição feito com sucesso.";

            return Redirect("SobreLivro?ISBN=" + ISBN);
        }
    }
}
