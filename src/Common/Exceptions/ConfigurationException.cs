namespace Common.Exceptions
{
    public sealed class ConfigurationException : BaseException
    {
        public ConfigurationException(string message) : base(message)
        {
        }

        public ConfigurationException(string messsage, Exception? innerExcepetion) : base(messsage, innerExcepetion)
        {
        }

        public static void Validate(bool hasError, string message, Exception? innerExcepetion = null)
        {
            if (hasError && !string.IsNullOrEmpty(message))
            {
                throw new ConfigurationException(message, innerExcepetion);
            }
        }
    }
}
