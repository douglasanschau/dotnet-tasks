using System.ComponentModel.DataAnnotations;

namespace tarefaUsuariosDotnet.Models;

public class LoginViewModel 
{   
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    public string Email { get; set; } = "";
    
    [Required(ErrorMessage = "O senha é obrigatória")]
    public string Senha { get; set; } = "";
}