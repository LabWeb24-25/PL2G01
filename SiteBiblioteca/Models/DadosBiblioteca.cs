using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class DadosBiblioteca
    {
        public string? Id { get; set; }

        [Required]
        public string? contactos { get; set; }

        [Required]
        public string? horario { get; set; }
    }
}
