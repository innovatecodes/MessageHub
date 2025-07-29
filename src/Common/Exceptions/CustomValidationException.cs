namespace Common.Exceptions
{
    public sealed class CustomValidationException : BaseException
    {
        public CustomValidationException(string message) : base(message)
        {
        }

        public CustomValidationException(string messsage, Exception? innerExcepetion) : base(messsage, innerExcepetion)
        {
        }

        public static void Validate(bool hasError, string message, Exception? innerExcepetion = null)
        {
            if (hasError && !string.IsNullOrEmpty(message))
            {
                throw new CustomValidationException(message, innerExcepetion);
            }
        }
    }
}
