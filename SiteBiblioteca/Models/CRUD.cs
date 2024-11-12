using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class CRUD
    {
        public int id { get; set; }

        [Required]
        public int id_Bibliotecario { get; set; }

        [Required]
        public string? operationType { get; set; }

        [Required]
        public string? ISBN { get; set; }
    }
}
