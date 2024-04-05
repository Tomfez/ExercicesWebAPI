namespace JobOverview.Exceptions
{
    public class ValidationRulesException: Exception
    {
        public Dictionary<string, string[]> Errors { get; } = new();
        public ValidationRulesException() { }

        public ValidationRulesException(string message) : base(message) { }

        public ValidationRulesException(string message,  Exception innerException) : base(message, innerException) { }

    }
}
