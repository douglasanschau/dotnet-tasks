using Microsoft.Extensions.DependencyInjection;
using tarefaUsuariosDotnet.Services;
using tarefaUsuariosDotnet.Data;

namespace tarefaUsuariosDotnet.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<UsuarioService>();
        services.AddScoped<TarefaService>();

        // Data
        services.AddScoped<Database>();

        return services;
    }
}