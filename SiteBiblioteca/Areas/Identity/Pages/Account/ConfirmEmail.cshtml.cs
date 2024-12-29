using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SiteBiblioteca.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailsender;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailsender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Obter o nome de utilizador da sessão
            var username = User.Identity.Name;

            // Encontrar o utilizador no banco de dados
            var aspnetuser = await _userManager.FindByNameAsync(username);
            if (aspnetuser == null)
            {
                StatusMessage = "Utilizador não encontrado.";
                return Page();
            }

            // Gerar o token de confirmação de email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(aspnetuser);

            // Criar um link de confirmação com o token gerado
            var callbackUrl = Url.Action("EmailConfirmado", "Pages",
                new { userId = aspnetuser.Id, code = token },
                protocol: Request.Scheme);

            var mensagem = $"<p>Olá,</p>" +
               "<p>Recebemos um pedido para confirmar o seu endereço de e-mail. Para prosseguir, clique no link abaixo:</p>" +
               $"<p><a href='{callbackUrl}'>Clique aqui para confirmar seu e-mail</a></p>" +
               "<p>Se você não solicitou isso, por favor, ignore este e-mail.</p>";

            await _emailsender.SendEmailAsync(aspnetuser.Email, "Confirme seu e-mail", mensagem);

            // Definir a mensagem de status
            StatusMessage = "Verifique seu e-mail para confirmar seu endereço de e-mail.";
            return Page();
        }
    }
}
