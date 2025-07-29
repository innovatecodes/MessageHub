using Common.Interfaces;

namespace Common.Constants
{
    public static class MessageTemplates
    {
        public static string Html(IInputRequest request, string subject) => $@"
            <div style='font-family: Arial, sans-serif; background-color: #fdfdfd; padding: 1rem; border: 2px solid #dd4b25; margin: auto; max-width: max-content;'>
                <h3 style='color: #dd4b25;'>Mensagem recebida via formulário</h3>

                <hr style='max-width: 50%;background-color: #dd4b25;height: 2px;border: none;'/>

                <p><b>Nome:</b> {GetDisplayName(request)}</p> 
                <p><b>E-mail:</b> {request.Email}</p>
                {(!string.IsNullOrWhiteSpace(request.WhatsApp) ? $"<p><b>WhatsApp:</b> {request.WhatsApp}</p>" : string.Empty)}
                <p><b>Assunto:</b> {subject}</p>
                <p><b>Mensagem:</b> {request.Message}</p>
            </div>
            ";

        public static string PlainText(IInputRequest request, string subject) => $"Mensagem recebida via formulário\n\nNome: {GetDisplayName(request)}\nE-mail: {request.Email}{(!string.IsNullOrWhiteSpace(request.WhatsApp) ? $"\nWhatsApp: {request.WhatsApp}" : string.Empty)}\nAssunto: {subject}\nMensagem: {request.Message}";

        private static string GetDisplayName(IInputRequest request) => !string.IsNullOrEmpty(request.Name) ? request.Name : request.Email;
    }
}
