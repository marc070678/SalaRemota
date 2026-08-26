using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalaRemota.Infrastructure.Persistence;

namespace SalaRemota.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SalaRemota");

        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            services.AddDbContext<SalaRemotaDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        return services;
    }
}
