﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteBiblioteca.Areas.Identity.Pages.Account;
using SiteBiblioteca.Data;
using SiteBiblioteca.Models;
using System.Drawing;

namespace SiteBiblioteca.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public PagesController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _emailSender = emailSender;
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
        public async Task<IActionResult> adicionarLivro(string isbn, string titulo, int autor, string generolivro, decimal preco, 
            int numexemplares, string sinopse, IFormFile Capa)
        {
            var _livro = new Livro
            {
                imagem = "/img/" + Guid.NewGuid().ToString() + Path.GetExtension(Capa.FileName),
                ISBN = isbn,
                titulo = titulo,
                sinopse = sinopse,
                autor = _context.autores.First(x => x.Id == autor),
                genero = generolivro,
                preco = preco,
                numExemplares = numexemplares
            };

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
                }

                // Salvar no banco de dados
                _context.livros.Add(_livro);
                await _context.SaveChangesAsync();

                // Redirecionar após sucesso
                return Redirect("/Pages/PainelBibliotecario");
            }

            // Caso os dados não sejam válidos, retornar o formulário com os erros
            return View(_livro);
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

        [HttpPost]
        public async Task<IActionResult> GuardarDadosBiblioteca(IFormFile image, string contactos, string horario, string mapa, string facebook, 
            string twitter, string instagram, string youtube, string tiktok)
        {
            var dados = _context._dadosBiblioteca.FirstOrDefault();

            if (dados == null)
            {
                dados = new DadosBiblioteca();
                dados.Id = "1";
                _context._dadosBiblioteca.Add(dados);
            }

            // Guardar a imagem
            if (image != null && image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                dados.image = image.FileName;
            }

            // Atualizar os dados restantes
            dados.contactos = contactos;
            dados.horario = horario;
            dados.mapa = mapa;
            dados.facebook = facebook;
            dados.x = twitter;
            dados.instagram = instagram;
            dados.youtube = youtube;
            dados.tiktok = tiktok;

            await _context.SaveChangesAsync(); // Salvar mudanças na tabela DadosBiblioteca

            return Redirect("/Pages/PainelAdministrador");
        }

        //[Authorize(Roles = "Administrador")]
        public IActionResult PainelAdministrador()
        {
            HttpContext.Session.SetString("Source", "PainelAdministrador");

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
            var bibliotecariosPorConfirmar = _context.Adicional.Where(x => x.confirmado == false); 

            return View(bibliotecariosPorConfirmar);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarBibliotecario(int id)
        {
            var bibliotecarioAdicional = await _context.Adicional.FirstAsync(x => x.Id == id);

            bibliotecarioAdicional.confirmado = true;

            await _context.SaveChangesAsync();

            return RedirectToAction("NotificacoesAdministrador");
        }

        [HttpPost]
        public async Task<IActionResult> NegarBibliotecario(int id)
        {
            var negadoAdicional = await _context.Adicional.FirstAsync(x => x.Id == id);

            await _emailSender.SendEmailAsync(negadoAdicional.Email, "Confirmação de Conta Negada", $"A sua conta foi negada! Qualquer reclamação, falar com a instituição.");

            // Remoção do Utilizador da base de dados
            var bibliotecarioUser = _context.Users.First(x => x.Email == negadoAdicional.Email);

            _context.Users.Remove(bibliotecarioUser);
            _context.Adicional.Remove(negadoAdicional);
            await _context.SaveChangesAsync();

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
        public async Task<IActionResult> guardarPerfil(string novousername, string nome, string email, string morada, string contactos, IFormFile profilePicture)
        {
            var username = User.Identity.Name; // Obter o username do utilizador autenticado

            // Verificar se o novo username já existe na tabela AspNetUsers (excluindo o próprio utilizador)
            var usernameExiste = await _context.Users.AnyAsync(x => x.UserName == novousername && x.UserName != username);
            if (usernameExiste)
            {
                // Retornar uma mensagem de erro para a view se o username já estiver em uso
                TempData["ErrorMessage"] = "O username já está em uso. Por favor, escolha outro.";
                return RedirectToAction("EditarPerfil"); // Redirecionar de volta para a página de edição
            }

            // Verificar se o utilizador existe na tabela AspNetUsers
            var useraspnet = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            // Obter os dados adicionais do utilizador na tabela Adicional
            var adicional = await _context.Adicional.FirstOrDefaultAsync(x => x.Email == useraspnet.Email);

            //parte a alterar
            if (profilePicture != null && profilePicture.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profilePicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(fileStream);
                }

                // Atualizar a imagem de perfil no modelo do usuário
                adicional.image = $"/img/{fileName}";
                await _context.SaveChangesAsync();
            }

            //fim da alteração

            // Atualizar os dados no AspNetUsers
            useraspnet.UserName = novousername;
            useraspnet.NormalizedUserName = novousername.ToUpper();
            await _context.SaveChangesAsync();

            // Atualizar os dados no Adicional
            adicional.Name = nome;
            adicional.Address = morada;
            adicional.Contact = contactos;
            await _context.SaveChangesAsync();

            // Forçar re-login do utilizador para atualizar o cookie de autenticação
            await _signInManager.SignOutAsync();  // Deslogar o utilizador
            await _signInManager.SignInAsync(useraspnet, isPersistent: false);  // Logar novamente o utilizador

            // Redirecionar para a página PersonalData
            return Redirect("/Identity/Account/PersonalData");
        }

        public IActionResult EmailConfirmado()
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
