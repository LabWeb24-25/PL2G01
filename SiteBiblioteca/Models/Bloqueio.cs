using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteBiblioteca.Models
{
    public class Bloqueio
    {
        public int userId { get; set; }

        public int adminId { get; set; }

        public DateTime dataBloqueio { get; set; }

        [Required]
        public string? motivo { get; set; }
    }
}