using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteBiblioteca.Models
{
    public class Requisitar
    {
        public User leitor { get; set; }

        public Livro livro { get; set; }

        public DateTime data_requisicao { get; set; }

        public DateTime data_entrega { get; set; }

        [Required]
        public User biblioEntrega { get; set; }

        public User biblioRecebe { get; set; }
    }
}
