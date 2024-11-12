using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class CriarAdministrador
    {
        [Key]
        [Required]
        public int idCriador { get; set; }

        [Key]
        [Required]
        public int idCriado { get; set; }
    }
}
