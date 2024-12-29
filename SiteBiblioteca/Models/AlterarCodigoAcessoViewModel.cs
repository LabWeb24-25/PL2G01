using System.ComponentModel.DataAnnotations;

public class AlterarCodigoAcessoViewModel
{
    [Required]
    public string Token { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Novo Código de Acesso")]
    public string NovoCodigo { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Código de Acesso")]
    [Compare("NovoCodigo", ErrorMessage = "Os códigos não coincidem.")]
    public string ConfirmarCodigo { get; set; }
}