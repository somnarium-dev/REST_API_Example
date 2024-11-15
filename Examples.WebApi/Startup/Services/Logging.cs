using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

namespace Examples.WebApi.Startup.Services
{
    public static class Logging
    {
        public static void AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            //Configure telemetry.
            TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.ConnectionString = configuration.GetConnectionString("AppInsights");
            TelemetryClient telemetryClient = new TelemetryClient(telemetryConfiguration);

            services.AddSingleton(telemetryClient);
            services.AddSingleton<TelemetryClient>();

            //Configure Serilog.
            string serilogOutputFormatDefault = "[{Timestamp:HH:mm:ss:ms} {Level:u3}] {Message:lj}{NewLine}{Exception}";
            string? serilogOutputFormat = configuration.GetValue<string>("SerilogOutputFormat") ?? serilogOutputFormatDefault;

            var serilogLogger = new LoggerConfiguration()
                .Enrich.WithProperty("Application", "Examples.REST_API.API")
                .Enrich.FromLogContext()
                .WriteTo.Console()
                //Text
                .WriteTo.Logger(textlogConsole => textlogConsole
                    .Enrich.FromLogContext()
                    .WriteTo.File(Path.Combine(@"C:\Logs\Examples_REST_API\API", "example_rest_api_api_.txt"),
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: serilogOutputFormat))
                .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
                .CreateLogger();

            services.AddSingleton<Serilog.ILogger>(serilogLogger);
        }
    }
}
