using Dapper;
using tarefaUsuariosDotnet.Data;
using tarefaUsuariosDotnet.Models;

namespace tarefaUsuariosDotnet.Services;

public class TarefaService
{
    private readonly Database _database;

    public TarefaService(Database database)
    {
        _database = database;
    }

    public List<TarefaModel> BuscarTarefasPorUsuario(int? usuarioId)
    {
        using var conn = _database.GetConnection();

        return conn.Query<TarefaModel>(
            @"SELECT
                id,
                titulo,
                descricao,
                usuario_id,
                concluida
            FROM tarefa
            WHERE usuario_id = @UsuarioId
            ORDER BY id DESC",
            new
            {
                UsuarioId = usuarioId
            }
        ).ToList();
    }

    public TarefaModel? BuscarTarefaPorId(int? tarefaId)
    {
        using var conn = _database.GetConnection();

        return  conn.QueryFirstOrDefault<TarefaModel>(
            @"SELECT *
            FROM tarefa
            WHERE id = @Id",
            new { Id = tarefaId }
        );
    }

    public int CadastrarTarefa(TarefaModel tarefa)
    {
        using var conn = _database.GetConnection();

        return conn.ExecuteScalar<int>(
            @"INSERT INTO tarefa
            (
                usuario_id,
                titulo,
                descricao
            )
            VALUES
            (
                @UsuarioId,
                @Titulo,
                @Descricao
            );

            SELECT LAST_INSERT_ID();",
            tarefa
        );
    }

    public int AtualizarTarefa(TarefaModel tarefa)
    {
        using var conn = _database.GetConnection();

        return  conn.Execute(
            @"UPDATE tarefa 
                SET
                    titulo    = @Titulo,
                    descricao = @Descricao
                WHERE id = @Id",
                tarefa
        );
    }


    public int ExcluirTarefa(int tarefaId)
    {
        using var conn = _database.GetConnection();

        return conn.Execute(
            @"DELETE FROM tarefa 
            WHERE id = @Id",
            new { Id = tarefaId }
        );
    }

}