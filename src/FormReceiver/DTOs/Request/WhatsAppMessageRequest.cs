using System.Text.Json.Serialization;

namespace FormReceiver.DTOs.Request
{
    public sealed class WhatsAppMessageRequest
    {
        [JsonPropertyName("messaging_product")]
        public string MessagingProduct { get; init; } = "whatsapp";
        [JsonPropertyName("recipient_type")]
        public string RecipientType { get; init; } = "individual";
        [JsonPropertyName("to")]
        public string To { get; init; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; init; } = "text";
        [JsonPropertyName("text")]
        public TextContent Text { get; init; } = new TextContent();
    }

    public sealed class TextContent 
    {
        [JsonPropertyName("preview_url")]
        public bool PreviewUrl { get; init; } = default;
        [JsonPropertyName("body")]
        public string Body { get; init; } = string.Empty;
    }

}

