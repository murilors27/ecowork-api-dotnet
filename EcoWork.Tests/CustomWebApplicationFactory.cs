using EcoWork.Api;
using EcoWork.Api.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Forçar ambiente Testing
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove o DbContext configurado no Program.cs
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<EcoWorkDbContext>)
            );

            if (descriptor != null)
                services.Remove(descriptor);

            // Adiciona banco de testes InMemory
            services.AddDbContext<EcoWorkDbContext>(options =>
            {
                options.UseInMemoryDatabase("EcoWorkTests");
            });

            // Garantir que o banco em memória começa limpo
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EcoWorkDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}