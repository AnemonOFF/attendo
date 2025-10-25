using Attendo.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Attendo.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            var cs = config.GetConnectionString("Default");
            services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(cs));
            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            return services;
        }
    }
}
