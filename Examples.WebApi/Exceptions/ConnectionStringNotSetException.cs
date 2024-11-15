namespace Examples.WebApi.Exceptions
{
    public class ConnectionStringNotSetException(string stringAtIssue) : Exception($"Connection string {stringAtIssue} not set!")
    {
    }
}