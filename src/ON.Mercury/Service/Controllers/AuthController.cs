using System.Buffers;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Models.Auth;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database.Entities;
using ON.Mercury.Service.Database.Repositories;
using ON.Mercury.Service.Services;

namespace ON.Mercury.Service.Controllers
{
    [ApiController]
    [Route("/api/{Controller}")]
    public class AuthController :  ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly PostgresContext _postgres;         // TODO: Replace with new MemberService
        private readonly MemberRepository _members;
        
        public AuthController(ILogger<AuthController> logger, PostgresContext postgres, MemberRepository members)
        {
            _logger = logger;
            _postgres = postgres;
            _members = members;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var user = ONUserHelper.ParseUser(HttpContext);
            
            if (user is not null)
            {
                var username = user.ToClaims().Where(c => c.Type == "Display").Select(c => c.Value).FirstOrDefault();
                var member = await _members.GetOrCreateMember(user.Id.ToString(), username);
                return Ok(new AuthenticateResponse()
                {
                    IsSuccess = true,
                    Errors = "",
                    Member = member
                });
            }
            
            return BadRequest(new AuthenticateResponse()
            {
                IsSuccess = false,
                Errors = "No User Data"
            });
        }
    }
}
