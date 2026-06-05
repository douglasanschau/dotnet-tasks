using System.ComponentModel.DataAnnotations;

namespace tarefaUsuariosDotnet.Models;

public class PerfilViewModel 
{   
    public int Id{ get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    public string Email { get; set; } = "";

    public string? Senha { get; set; } = "";

    [Compare("Senha", ErrorMessage = "A senha e confirmar senha devem ser iguais.")]
    public string? ConfirmarSenha { get; set; } = "";
}