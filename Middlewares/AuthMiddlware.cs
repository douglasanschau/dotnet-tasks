namespace tarefaUsuariosDotnet.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var usuarioId = context.Session.GetInt32("UsuarioId");

        if (usuarioId == null &&
            !context.Request.Path.StartsWithSegments("/web/login") &&
            !context.Request.Path.StartsWithSegments("/web/registro"))
        {
            context.Response.Redirect("/web/login");
            return;
        }

        if(usuarioId != null && 
            (context.Request.Path.StartsWithSegments("/web/login") ||
            context.Request.Path.StartsWithSegments("/web/registro")))
        {
            context.Response.Redirect("/web/painel");
            return;
        }

        await _next(context);
    }
}