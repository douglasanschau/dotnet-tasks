using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tarefaUsuariosDotnet.Models;
using tarefaUsuariosDotnet.Services;
using Microsoft.AspNetCore.Identity;

namespace tarefaUsuariosDotnet.Controllers;

[Route("web")]
public class UsuarioController : Controller
{
    private readonly UsuarioService _usuarioService;
    private readonly TarefaService  _tarefaService;

    public UsuarioController(UsuarioService usuarioService, TarefaService tarefaService)
    {
        _usuarioService = usuarioService;
        _tarefaService  = tarefaService;
    }   

    [HttpGet("login")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel  model)
    {
        if (!ModelState.IsValid)
            return View("Index", model);

        UsuarioModel? usuario = await _usuarioService.buscaPorEmail(model.Email);

        if(usuario is not null){
                var hasher = new PasswordHasher<UsuarioModel>();

                var resultado = hasher.VerifyHashedPassword(
                    usuario,
                    usuario.Senha,
                    model.Senha
                );

                if (resultado == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32(
                        "UsuarioId",
                        usuario.Id
                    );
                    
                    return RedirectToAction("Painel");
                }
        }

        // ❌ aqui vai dar erro agora (sem usuários no banco)
        ModelState.AddModelError("", "Usuário ou senha inválidos");
        return View("Index", model);
    }

    [HttpGet("registro")]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost("registro")]
    public async Task <IActionResult> Registro(RegistroViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Registro", model);

        var usuarioExistente = await _usuarioService.buscaPorEmail(model.Email);

        if (usuarioExistente is not null){
            ModelState.AddModelError(
                "Email",
                "Este e-mail já está cadastrado"
            );

            return View("Registro", model);
        }

        var hasher = new PasswordHasher<UsuarioModel>();

        var usuario = new UsuarioModel {
            Nome = model.Nome,
            Email = model.Email,
        };

        usuario.SenhaHash = hasher.HashPassword(usuario, model.Senha);

        var usuario_id = _usuarioService.SalvarUsuario(usuario);

        return RedirectToAction("Index");
    }

    [HttpGet("painel")]
    public IActionResult Painel()
    {
        int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
        
        var tarefas = _tarefaService.BuscarTarefasPorUsuario(usuarioId);

        return View(tarefas);
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {   
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }


    [HttpGet("tarefa/{tarefaId?}")]
    public IActionResult Tarefa(int? tarefaId)
    {
        var tarefa = tarefaId != null ? _tarefaService.BuscarTarefaPorId(tarefaId) : null;
        return View(tarefa);
    }

    [HttpPost("tarefa")]
    public IActionResult SalvarTarefa(TarefaModel model)
    {
        if (!ModelState.IsValid)
            return View("Tarefa", model);

        model.UsuarioId = HttpContext.Session.GetInt32("UsuarioId").Value;

        if(model.Id > 0){
           _tarefaService.AtualizarTarefa(model);
        } else {
           _tarefaService.CadastrarTarefa(model);
        }

        return RedirectToAction("Painel");
    }

    [HttpPost("ExcluirTarefa/{tarefaId}")]
    public IActionResult ExcluirTarefa(int tarefaId)
    {
        _tarefaService.ExcluirTarefa(tarefaId);
        return RedirectToAction("Painel");
    }

    [HttpGet("Perfil")]
    public IActionResult Perfil()
    {
       int usuarioId =  HttpContext.Session.GetInt32("UsuarioId").Value;
       var usuario   =  _usuarioService.BuscaUsuarioPorId(usuarioId);

       var model = new PerfilViewModel {
         Id = usuario.Id,
         Nome = usuario.Nome,
         Email = usuario.Email,
       };

       return View(model);
    }

    [HttpPost("Perfil")]
    public async Task <IActionResult> Perfil(PerfilViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Perfil", model);

        int usuarioId = HttpContext.Session.GetInt32("UsuarioId").Value;
        var usuarioExistente = await _usuarioService.buscaPorEmail(model.Email);


        if(usuarioExistente is not null){
            if(usuarioExistente.Id != usuarioId){
                 ModelState.AddModelError(
                    "Email",
                    "Este e-mail já está cadastrado"
                );

                return View("Perfil", model);
            }
        }

        
        var usuario = new UsuarioModel {
            Id    = usuarioId,
            Nome  = model.Nome,
            Email = model.Email,
        };

        if (!string.IsNullOrWhiteSpace(model.Senha)){
            var hasher = new PasswordHasher<UsuarioModel>();
            usuario.SenhaHash = hasher.HashPassword(usuario, model.Senha);
        } else {
            var usuarioAtual = _usuarioService.BuscaUsuarioPorId(usuarioId);
            usuario.SenhaHash = usuarioAtual.Senha;
        }

        _usuarioService.AtualizarUsuario(usuario);

        return RedirectToAction("Painel");
    }

}