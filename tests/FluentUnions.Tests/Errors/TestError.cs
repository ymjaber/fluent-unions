namespace FluentUnions.Tests.Errors
{
    /// <summary>
    /// Test error class for testing metadata functionality
    /// </summary>
    public class TestError : ErrorWithMetadata
    {
        public TestError(string code, string message, IDictionary<string, object> metadata)
            : base(code, message, metadata)
        {
        }

        public TestError(string code, string message)
            : base(code, message)
        {
        }

        public TestError(string message)
            : base(message)
        {
        }
    }
}