namespace Common.Interfaces
{
    public interface IInputRequest
    {
        public string? Name { get; }
        public string Email { get; }
        public string? Phone { get; }
        public string? WhatsApp { get; }
        public string? Subject { get; }
        public string Message { get; }    
    }
}
