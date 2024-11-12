using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteBiblioteca.Models
{
    public class Requisitar
    {
        public int leitorId { get; set; }

        public int livroId { get; set; }

        public DateTime data_requisicao { get; set; }

        public DateTime data_entrega { get; set; }

        [Required]
        public int biblioEntregaId { get; set; }

        public int biblioRecebeId { get; set; }
    }
}
