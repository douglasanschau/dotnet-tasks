using System.ComponentModel.DataAnnotations;

namespace tarefaUsuariosDotnet.Models;

public class RegistroViewModel 
{   
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = "";

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    public string Email { get; set; } = "";
    
    [Required(ErrorMessage = "O senha é obrigatória")]
    public string Senha { get; set; } = "";
}