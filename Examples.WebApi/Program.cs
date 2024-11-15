using Examples.Data;
using Examples.WebApi.Services;
using Examples.WebApi.Startup.Services;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting.WindowsServices;

namespace Examples.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Get environment.
            string? environment = Environment.GetEnvironmentVariable("ENVIRONMENT")?.Trim();

            //Get Azure App Config connection string.
            string? azureAppConfigConnectionString = Environment.GetEnvironmentVariable("AZURE_CONFIG_CONNECTION_STRING")?.Trim();

            //Build configuration
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddAzureAppConfiguration(azureAppConfigConnectionString)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();

            //Create web api builder
            var options = new WebApplicationOptions()
            {
                Args = args,
                EnvironmentName = environment,
                ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
            };

            var builder = WebApplication.CreateBuilder(options);

            //Add services.
            builder.Services.AddControllers(options =>
            {
                //options.ReturnHttpNotAcceptable = true;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddExampleDbContext(configuration);
            builder.Services.AddLogging(configuration);
            builder.Services.AddScoped<IExDbParticipantManager, ExDbParticipantManager>();
            builder.Services.AddScoped<IExDbHistoryManager, ExDbHistoryManager>();

            #if (!DEBUG)
            builder.Host.UseWindowsService();
            #endif

            //Build.
            var app = builder.Build();

            // Ensure Db exists in its most recent iteration.
            ExampleDbContext context = app.Services.GetRequiredService<ExampleDbContext>();
            //context.Database.Migrate();

            //Ensure telemetry flushes on program termination.
            IHostApplicationLifetime lifetime = app.Lifetime;

            lifetime.ApplicationStopping.Register(() =>
                app.Services.GetRequiredService<TelemetryClient>().Flush()
            );

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            //app.UseAuthService();

            //app.UseMiddleware<AddUserNameMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
