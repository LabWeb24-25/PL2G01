namespace SiteBiblioteca.Models
{
    public class Utilizador
    {
        public int Id { get; set; }
        public string TipoUtilizador { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool banido { get; set; } // Indica se o usuário está banido
    }
}
