using ON.Mercury.Service.Database.Entities;

namespace ON.Mercury.Service.Models.Auth
{
    public class AuthenticateResponse
    {
        public bool IsSuccess { get; set; }
        public string Errors { get; set; } = "";
        public MemberEntity Member { get; set; }
    }
}
