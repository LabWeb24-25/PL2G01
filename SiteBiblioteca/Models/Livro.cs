using System.ComponentModel.DataAnnotations;
using SiteBiblioteca.Models;

namespace SiteBiblioteca.Models
{
    public class Livro
    {
        [Key]
        [Required]
        public string? ISBN { get; set; }

        [Required]
        public string? titulo { get; set; }

        [Required]
        public Autor? autor { get; set; }

        [Required]
        public string? genero { get; set; }

        [Required]
        public double preco { get; set; }

        [Required]
        public int numExemplares { get; set; }

        [Required]
        public string? sinopse { get; set; }

        [Required]
        public string? imagem { get; set; }
    }
}
