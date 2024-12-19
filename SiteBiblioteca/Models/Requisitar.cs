using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteBiblioteca.Models
{
    public class Requisitar
    {
        [ForeignKey("Adicional")]
        public int leitorId { get; set; } // Chave estrangeira
        
        [ForeignKey("Livro")]
        public int livroId { get; set; } // Chave estrangeira
   
        public DateTime data_requisicao { get; set; }
        public DateTime data_entrega { get; set; }

        [ForeignKey("Adicional")]
        public int biblioEntregaId { get; set; } // Chave estrangeira opcional

        [ForeignKey("Adicional")]
        public int biblioRecebeId { get; set; } // Chave estrangeira opcional
    }
}

