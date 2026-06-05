#
Projeto pessoal simples de cadastro de tarefas usando .NET para estudo de algumas concepções do .NET: Middlware, CRUD, Autenticação, Hash de senhas e Validações de formulários.
####

Para estrutura do projeto criei um banco de dados nomeado dotnet e dentro dele criei duas tabelas: usuario e tarefa.
Efetive conecção do banco incluindo no arquivo appsettings.json os dados para coneção. 

`
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=dotnet;Uid=${seu_usuario - normalmente root};Pwd=${sua_senha};"
  }
`

Tabela usuario 
- id
- nome
- email
- senha

Tabela tarefa 
- id
- usuario_id
- titulo
- descricao

###########

Inialização do Projeto
--
Dentro da pasta rodar o comando .dotnet run... 
Versão Dotnet usada para criação da aplicação : 8.0.421





