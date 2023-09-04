using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace ON.Mercury.Service.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        private const string Action = "Not Found";
        private string Type { get; set; }
        private string EntityId { get; set; }

        public NotFoundException(string type, string entityId) : base($"{type} {entityId} {Action} ")
        {
            Type = type;
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
