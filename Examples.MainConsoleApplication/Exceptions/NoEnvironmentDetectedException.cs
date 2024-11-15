namespace Examples.ConsoleApplication.Exceptions
{
    public class NoEnvironmentDetectedException : Exception
    {
        public NoEnvironmentDetectedException() : base("Environment Variable \"ENVIRONMENT\" not set (Expected \"LOCAL\", \"DEV\", or \"PROD\".")
        {

        }
    }
}
