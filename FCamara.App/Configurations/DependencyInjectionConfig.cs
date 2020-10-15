using FCamara.Bussiness.Interfaces;
using FCamara.Bussiness.Interfaces.Repository;
using FCamara.Bussiness.Interfaces.Service;
using FCamara.Bussiness.Notificacoes;
using FCamara.Bussiness.Services;
using FCamara.Data.Context;
using FCamara.Data.Repository;
using Microsoft.Extensions.DependencyInjection;


namespace FCamara.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();           
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            services.AddScoped<IDependenteRepository, DependenteRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepositoy>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IFuncionarioService, FuncionarioService>();
            services.AddScoped<IDependenteService, DependenteService>();

            return services;
        }
    }
}
