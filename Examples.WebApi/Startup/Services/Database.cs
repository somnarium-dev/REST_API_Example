using Examples.Data;
using Examples.WebApi.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Examples.WebApi.Startup.Services
{
    public static class Database
    {
        public static IServiceCollection AddExampleDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string? exampleDbConnectionString = configuration.GetConnectionString("ExampleDb");
            if (String.IsNullOrEmpty(exampleDbConnectionString)) { throw new ConnectionStringNotSetException("ExampleDb"); }

            services.AddDbContext<ExampleDbContext>(options =>
                options.UseSqlServer(exampleDbConnectionString)
            );

            return services;
        }
    }
}
