namespace Common.Exceptions
{
    public abstract class BaseException : Exception
    {
        public BaseException(string message) : base(message) { }

        public BaseException(string messsage, Exception? innerExcepetion) : base(messsage, innerExcepetion) { }
    }
}
