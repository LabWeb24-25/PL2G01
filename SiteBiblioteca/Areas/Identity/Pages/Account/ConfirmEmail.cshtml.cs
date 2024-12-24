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
            if (_emailsender == null)
                return Redirect("/");

            // Obter o nome de utilizador da sessão
            var username = User.Identity.Name;

            // Encontrar o utilizador no banco de dados
            var aspnetuser = await _userManager.FindByNameAsync(username);
            if (aspnetuser == null)
            {
                StatusMessage = "utilizador não encontrado.";
                return Page();
            }

            // Gerar um código de confirmação aleatório
            string confirmationCode = GenerateConfirmationCode();

            // Confirmar o e-mail do utilizador com o código gerado
            var result = await _userManager.ConfirmEmailAsync(aspnetuser, confirmationCode);

            // Criar um link de confirmação que será enviado por e-mail
            var callbackUrl = Url.Page(
                "/EmailConfirmado", // Página onde o utilizador será redirecionado após a confirmação
                pageHandler: null,
                values: new { userId = aspnetuser.Id, code = confirmationCode },
                protocol: Request.Scheme);

            // Enviar o código de confirmação ao utilizador por e-mail
            await _emailsender.SendEmailAsync(aspnetuser.Email, "Confirme seu e-mail", $"Clique no link para confirmar seu e-mail: {callbackUrl}");

            // Definir a mensagem de status com base no resultado da confirmação
            StatusMessage = result.Succeeded ? "Obrigado por confirmar seu e-mail." : "Erro ao confirmar seu e-mail.";
            return Page();
        }

        // Função para gerar um código de confirmação aleatório
        private string GenerateConfirmationCode()
        {
            var random = new Random();
            var builder = new StringBuilder();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            // Gerar um código de 32 caracteres
            for (int i = 0; i < 32; i++)
            {
                builder.Append(chars[random.Next(chars.Length)]);
            }
            return builder.ToString();
        }
    }
}
