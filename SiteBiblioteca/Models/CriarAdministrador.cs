using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteBiblioteca.Models
{
    public class CriarAdministrador
    {
        public int idCriador { get; set; }

        public int idCriado { get; set; }
    }
}
