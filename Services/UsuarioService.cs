using Dapper;
using tarefaUsuariosDotnet.Data;
using tarefaUsuariosDotnet.Models;

namespace tarefaUsuariosDotnet.Services;

public class UsuarioService
{
    private readonly Database _database;

    public UsuarioService(Database database)
    {
        _database = database;
    }

    public async Task<UsuarioModel?> buscaPorEmail(string email)
    {
        using var conn = _database.GetConnection();

        return await conn.QueryFirstOrDefaultAsync<UsuarioModel>(
            @"SELECT id,
                    nome,
                    email,
                    senha
            FROM usuario
            WHERE email = @Email",
            new { Email = email }
        );
    }


    public int SalvarUsuario(UsuarioModel usuario)
    {
        using var conn = _database.GetConnection();

        return conn.ExecuteScalar<int>(
            @"INSERT INTO usuario
            (
                nome,
                email,
                senha
            )
            VALUES
            (
                @Nome,
                @Email,
                @SenhaHash
            );

            SELECT LAST_INSERT_ID();",
            usuario
        );
    }

    public int AtualizarUsuario(UsuarioModel usuario)
    {
        using var conn = _database.GetConnection();

        return conn.Execute(
            @"UPDATE usuario
                SET
                    nome  = @Nome,
                    email = @Email,
                    senha = @SenhaHash
                WHERE id = @Id",
                usuario
        );
    }

    public UsuarioModel? BuscaUsuarioPorId(int? usuarioId)
    {
        using var conn = _database.GetConnection();

        return  conn.QueryFirstOrDefault<UsuarioModel>(
            @"SELECT *
            FROM usuario
            WHERE id = @Id",
            new { Id = usuarioId }
        );
    }
}