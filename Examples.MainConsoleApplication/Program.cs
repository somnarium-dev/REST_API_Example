using Examples.ConsoleApplication.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Examples.ConsoleApplication
{
    internal class Program
    {
        internal static void Main()
        {
            var services = ConfigureServices();

            ILogger logger = services.GetRequiredService<ILogger>();

            try
            {
                logger.Information("Serilog is functioning correctly.");

                var testFunction = services.GetRequiredService<PrimaryFunction>();
                testFunction.Execute();
            }
            catch (Exception ex)
            {
                logger.Fatal("Fatal Exception occurred: {exceptionMessage}", ex.Message);
            }
            finally
            {
                logger.Information("End execution.");
            }
        }

        static ServiceProvider ConfigureServices()
        {
            ServiceCollection services = new();

            //Get environment
            string? environment = Environment.GetEnvironmentVariable("ENVIRONMENT")?.Trim();
            if (string.IsNullOrEmpty(environment)) { throw new NoEnvironmentDetectedException(); }

            //Build configuration
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configurationBuilder.Build();

            // Configure Serilog.
            string serilogOutputFormat = configuration.GetValue<string>("SerilogOutputFormat") ?? "";
            string serilogOutputFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");


            //Serilog
            var serilogLogger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", "Examples.REST_API.Console")
                .Enrich.FromLogContext() // This line allows properties to be added/removed using LogContext.PushProperty();
                .WriteTo.Console()
                .WriteTo.Logger(textlogConsole => textlogConsole
                    .Enrich.FromLogContext()
                    .WriteTo.File(Path.Combine(serilogOutputFilepath, "examples_rest_api_console_.txt"),
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: serilogOutputFormat))
                .CreateLogger();

            // Add singletons and scoped services.
            services.AddSingleton<ILogger>(serilogLogger);

            services.AddScoped<PrimaryFunction>();

            return services.BuildServiceProvider();
        }
    }
}
