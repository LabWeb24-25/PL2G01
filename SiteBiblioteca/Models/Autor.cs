using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class Autor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Nome { get; set; }

        [Required]
        public string? Bibliografia { get; set; }

        [Required]
        public string? Imagem { get; set; }
    }
}
