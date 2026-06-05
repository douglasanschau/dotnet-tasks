using System.ComponentModel.DataAnnotations;

namespace tarefaUsuariosDotnet.Models;

public class TarefaModel
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "O titulo é obrigatório")]
    public string Titulo { get; set; } = "";

    [Required(ErrorMessage = "A descrição é obrigatória")]
    public string Descricao { get; set; } = "";

    public bool Concluida { get; set; }
}