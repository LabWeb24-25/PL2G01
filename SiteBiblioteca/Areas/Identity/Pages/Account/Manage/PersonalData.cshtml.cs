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
        public string? UserName { get; set; }
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? Contact { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public bool Confirmado { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Obter o ID do utilizador logado
            var userId = User?.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("Utilizador não encontrado.");
            }

            // Obter o utilizador do banco de dados (tabela AspNetUsers)
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userId);

            if (user == null)
            {
                return NotFound("Utilizador não encontrado.");
            }

            // Obter os dados adicionais do utilizador (tabela Adicional)
            var additionalInfo = await _context.Adicional.FirstOrDefaultAsync(a => a.Email == user.Email);

            if (additionalInfo == null)
            {
                return NotFound("Informações adicionais não encontradas.");
            }

            // Preencher as propriedades com os dados do utilizador
            UserName = user.UserName;
            Image = additionalInfo.image; // Assumindo que a coluna na tabela se chama 'Image'
            Name = additionalInfo.Name;
            Contact = additionalInfo.Contact;
            Address = additionalInfo.Address;
            Email = additionalInfo.Email;
            Confirmado = user.EmailConfirmed; // Confirmado será um valor booleano

            return Page(); // Apenas retorna a página sem redirecionar
        }
    }
}
