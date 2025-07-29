namespace Common.Constants
{
    public  static class AppConstants
    {
        public const string NAME_LENGTH_ERROR = "Nome deve ter no mínimo 4 caracteres!";
        public const string REQUIRED_EMAIL_ERROR = "E-mail é obrigatório!";
        public const string INVALID_EMAIL_ERROR = "E-mail inválido!";
        public const string WHATSAPP_LENGTH_ERROR = "Número de WhatsApp deve ter 11 dígitos, incluindo o DDD!";
        public const string SUBJECT_LENGTH_ERROR = "O assunto deve ter entre 6 e 40 caracteres!";
        public const string REQUIRED_MESSAGE_ERROR = "Mensagem é obrigatória!";
        public const string MESSAGE_LENGTH_ERROR = "Mensagem deve ter no mínimo 6 caracteres!";
        public const string INVALID_DATA_ERROR = "Dados inválidos!";
        public const string EMAIL_SENT = "E-mail enviado!";
        public const string EMAIL_AUTOREPLY_SENT = "E-mail de resposta automática enviado!";
        public const string EMAIL_SEND_FAILURE_ERROR = "Falha ao enviar e-mail!"; 
        public const string EMAIL_AUTOREPLY_FAILURE_ERROR = "Falha ao enviar e-mail de resposta automática!";
        public const string UNEXPECTED_ERROR = "Aconteceu um erro inesperado!";
        public const string EVENT_FAILURE_ERROR = "Falha ao processar o evento!";
        public const string SERVER_ERROR = "Erro interno do servidor!";
        public const string CONFIG_LOAD_FAILURE_ERROR = "Falha ao carregar as configurações: verifique o appsettings.json!";

        // ✅ Novas constantes para WhatsApp:
        public const string WHATSAPP_SENT = "WhatsApp enviado!";
        public const string WHATSAPP_SEND_FAILURE_ERROR = "Falha ao enviar WhatsApp!";
        public const string EXTERNAL_API_FAILURE_ERROR = "A requisição foi malformada ou inválida para a API externa.";
    }
}
