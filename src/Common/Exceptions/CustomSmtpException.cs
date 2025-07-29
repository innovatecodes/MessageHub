namespace Common.Exceptions
{
    public  class CustomSmtpException : BaseException
    {
        public CustomSmtpException(string message) : base(message)
        {
        }

        public CustomSmtpException(string messsage, Exception? innerExcepetion) : base(messsage, innerExcepetion)
        {
        }

        public static void Validate(bool hasError, string message, Exception? innerExcepetion = null)
        {
            if (hasError && !string.IsNullOrEmpty(message))
                throw new CustomSmtpException(message, innerExcepetion);
        }
    }
}

