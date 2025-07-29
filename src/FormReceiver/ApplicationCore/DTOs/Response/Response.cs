using Common.Enums;
using System.Text.Json.Serialization;

namespace FormReceiver.ApplicationCore.DTOs.Response
{
    //Versão mais concisa com record posicional
    public record Response([property: JsonConverter(typeof(JsonStringEnumConverter))] Status Status, string Message, List<string>? Errors) 
    {
        public Response(Status status, string message, params string[]? errors) : this(status, message, errors?.ToList() ?? []) { }
    }

    /*
    public record Response
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Status Status { get; init; }
        public string Message { get; init; } = string.Empty;
        public List<string>? Errors { get; init; }

        public Response(Status status, string message, params string[]? errors)
        {
            Status = status;
            Message = message;
            Errors = errors?.ToList() ?? []; //new List<string>();
        }
    }
    */
}
