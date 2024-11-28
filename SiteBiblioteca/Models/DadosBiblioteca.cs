using System.ComponentModel.DataAnnotations;

namespace SiteBiblioteca.Models
{
    public class DadosBiblioteca
    {
        public string? Id { get; set; }

        [Required]
        public string? image { get; set; }

        [Required]
        public string? contactos { get; set; }

        [Required]
        public string? horario { get; set; }

        [Required]
        public string? mapa { get; set; }

        public string? facebook { get; set; }

        public string? x { get; set; }

        public string? instagram { get; set; }

        public string? youtube { get; set; }

        public string? tiktok { get; set; }
    }
}