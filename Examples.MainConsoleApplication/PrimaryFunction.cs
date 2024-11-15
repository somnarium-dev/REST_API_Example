using Serilog;

namespace Examples.ConsoleApplication
{
    public class PrimaryFunction(ILogger logger)
    {
        public ILogger _logger = logger;

        public void Execute()
        {
            _logger.Information("Primary function executed successfully.");
        }
    }
}