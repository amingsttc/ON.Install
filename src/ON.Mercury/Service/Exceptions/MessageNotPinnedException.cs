using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace ON.Mercury.Service.Exceptions
{
    public class MessageNotPinnedException : Exception
    {
        private const string Action = "Not Found";
        private const string Type = "Pinned Message";
        private string EntityId { get; set; }
        public MessageNotPinnedException(string entityId) : base($"Message {entityId} not pinned")
        {
            EntityId = entityId;
        }

        public ProblemDetails GetDetails()
        {
            return new ProblemDetails()
            {
                Status = (int)HttpStatusCode.NotFound,
                Type = Type,
                Title = $"{Type} {Action}",
                Detail = $"{Type} {EntityId} {Action}"
            };
        }
    }
}
