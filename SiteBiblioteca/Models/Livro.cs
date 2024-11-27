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
        [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser maior ou igual a 0.")]
        public decimal preco { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "O número de exemplares deve ser maior ou igual a 0.")]
        public int numExemplares { get; set; }

        [Required]
        public string? sinopse { get; set; }

        [Required]
        public string? imagem { get; set; }
    }
}
