namespace JobOverview.Exceptions
{
    public class ValidationRulesException: Exception
    {
        public Dictionary<string, string[]> Errors { get; } = new();
        public ValidationRulesException() { }

        public ValidationRulesException(string property, string message) : base(message) 
        {
            Errors.Add(property, new string[] { message });
        }

        public ValidationRulesException(string property, string message,  Exception innerException) : base(message, innerException) 
        {
            Errors.Add(property, new string[] { message });
        }

    }
}
