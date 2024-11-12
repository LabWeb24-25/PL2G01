using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class Bloqueio
    {
        [Key]
        public int userId { get; set; }

        [Key]
        public int adminId { get; set; }

        [Key]
        public DateTime dataBloqueio { get; set; }

        [Required]
        public string? motivo { get; set; } 
    }
}
