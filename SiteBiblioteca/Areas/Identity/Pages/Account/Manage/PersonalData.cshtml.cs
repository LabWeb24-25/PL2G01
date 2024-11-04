using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SiteBiblioteca.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]  // Adiciona este atributo para permitir acesso anônimo

    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public PersonalDataModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Propriedades para exibir na view
        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Address { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public bool EmailConfirmed { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Preenchendo as propriedades com os dados do usuário
            UserName = user.UserName;
            Email = user.Email;
            EmailConfirmed = user.EmailConfirmed;

            // Aqui, assumimos que FullName e Address são armazenados como Claims.
            // Se estiverem em outra estrutura, adapte o código conforme necessário.
            var claims = await _userManager.GetClaimsAsync(user);
            FullName = claims.FirstOrDefault(c => c.Type == "FullName")?.Value ?? "N/A";
            Address = claims.FirstOrDefault(c => c.Type == "Address")?.Value ?? "N/A";
            PhoneNumber = user.PhoneNumber ?? "N/A";

            return Page();
        }
    }
}
