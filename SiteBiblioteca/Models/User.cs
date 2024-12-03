using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? UserType { get; set; }

        [Required]
        public string? image { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Contact { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public bool? confirmado { get; set; }

        [Required]
        public bool? banido { get; set; } = false;
    }
}
