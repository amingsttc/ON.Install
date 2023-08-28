using Newtonsoft.Json;
using ON.Mercury.Service.Database.Entities;
using Service.Database.Entities;

namespace ON.Mercury.Service.Models.Auth
{
    public class AuthenticateResponse
    {
        public bool IsSuccess { get; set; }
        public string Errors { get; set; } = "";
        public Member Member { get; set; }
    }
}
