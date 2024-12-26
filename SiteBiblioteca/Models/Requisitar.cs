using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteBiblioteca.Models
{
    public class Requisitar
    {
        [ForeignKey("leitor")]
        public int leitorId { get; set; } // Chave estrangeira
        public User leitor { get; set; }

        [ForeignKey("Livro")]
        public string? livroISBN { get; set; } // Chave estrangeira
        public Livro livro { get; set; }

        public DateTime data_requisicao { get; set; }

        public DateTime data_entrega { get; set; }

        [ForeignKey("biblioEntrega")]
        public int? biblioEntregaId { get; set; } // Chave estrangeira opcional
        public User? biblioEntrega { get; set; }

        [ForeignKey("biblioRecebe")]
        public int? biblioRecebeId { get; set; } // Chave estrangeira opcional
        public User? biblioRecebe { get; set; }
    }
}

