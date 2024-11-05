using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? UserType { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Contact { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Email { get; set; }
    }
}
