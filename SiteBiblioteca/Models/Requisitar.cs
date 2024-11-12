using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class Requisitar
    {
        [Key]
        public int leitorId { get; set; }

        [Key]
        public int livroId { get; set; }

        [Key]
        public DateTime data_requisicao { get; set; }

        [Required]
        public DateTime data_entrega { get; set; }

        [Required]
        public int biblioEntregaId { get; set; }

        [Required]
        public int biblioRecebeId { get; set; }
    }
}
