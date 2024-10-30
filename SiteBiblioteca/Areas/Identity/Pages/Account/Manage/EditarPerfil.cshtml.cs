using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace SiteBiblioteca.Areas.Identity.Pages.Account.Manage
{
    public class EditarPerfilModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EditarPerfilModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Nome { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Morada { get; set; }

        [BindProperty]
        public string Contactos { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Preencher os campos do modelo com os dados do usuário
            Username = user.UserName;
            Email = user.Email;

            // Recuperar claims específicas (Nome, Morada e Contactos)
            var claims = await _userManager.GetClaimsAsync(user);
            Nome = claims.FirstOrDefault(c => c.Type == "Nome")?.Value;
            Morada = claims.FirstOrDefault(c => c.Type == "Morada")?.Value;
            Contactos = claims.FirstOrDefault(c => c.Type == "Contactos")?.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.UserName = Username;
            user.Email = Email;

            // Atualizar claims adicionais (Nome, Morada e Contactos)
            var claims = await _userManager.GetClaimsAsync(user);
            await UpdateClaimAsync(user, claims, "Nome", Nome);
            await UpdateClaimAsync(user, claims, "Morada", Morada);
            await UpdateClaimAsync(user, claims, "Contactos", Contactos);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            // Atualizar o sign-in do usuário para aplicar as mudanças
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Seu perfil foi atualizado com sucesso.";
            return RedirectToPage();
        }

        // Método auxiliar para atualizar claims
        private async Task UpdateClaimAsync(IdentityUser user, IList<Claim> claims, string claimType, string claimValue)
        {
            var existingClaim = claims.FirstOrDefault(c => c.Type == claimType);
            if (existingClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, existingClaim);
            }
            await _userManager.AddClaimAsync(user, new Claim(claimType, claimValue));
        }
    }
}
