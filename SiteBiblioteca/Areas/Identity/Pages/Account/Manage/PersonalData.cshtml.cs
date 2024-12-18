using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SiteBiblioteca.Models;
using SiteBiblioteca.Data;

namespace SiteBiblioteca.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PersonalDataModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Propriedades para exibir na view
        [BindProperty]
        public string? UserName { get; set; }

        [BindProperty]
        public string? Image { get; set; }

        [BindProperty]
        public string? Name { get; set; }

        [BindProperty]
        public string? Contact { get; set; }

        [BindProperty]
        public string? Address { get; set; }

        [BindProperty]
        public string? Email { get; set; }

        [BindProperty]
        public bool? Confirmado { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Obter o ID do utilizador logado
            var userId = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("utilizador não encontrado.");
            }

            // Obter o utilizador do banco de dados (tabela AspNetUsers)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userId);

            if (user == null)
            {
                return NotFound("utilizador não encontrado.");
            }

            // Obter os dados adicionais do utilizador (tabela Adicional)
            var additionalInfo = await _context.Adicional.FirstOrDefaultAsync(a => a.Email == user.Email);

            if (additionalInfo == null)
            {
                return NotFound("Informações adicionais não encontradas.");
            }

            // Preencher as propriedades com os dados do utilizador
            UserName = user.UserName;
            Image = additionalInfo.image;
            Name = additionalInfo.Name;
            Contact = additionalInfo.Contact;
            Address = additionalInfo.Address;
            Email = additionalInfo.Email;
            Confirmado = additionalInfo.confirmado;

            return RedirectToPage("/Account/Manage/PersonalData");
        }
    }
}
