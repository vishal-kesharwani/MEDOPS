using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MedOps.Infrastructure.Data;

namespace MedOps.ApiTests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MedOpsDbContext>));
            if (dbDescriptor != null)
                services.Remove(dbDescriptor);

            var dbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions) ||
                d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>));
            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            var allDbDescriptors = services.Where(d =>
                d.ServiceType.FullName?.Contains("EntityFrameworkCore") == true).ToList();
            foreach (var d in allDbDescriptors)
                services.Remove(d);

            services.AddDbContext<MedOpsDbContext>(options =>
                options.UseInMemoryDatabase("MedOpsTestDb"));
        });
    }
}
