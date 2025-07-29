namespace Common.Interfaces
{
    public interface IEmailService<TRequest, TResponse> 
    {
        public Task<TResponse> SendEmailAsync(TRequest request);
    }
}

