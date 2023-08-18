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
using ON.Mercury.Service.Services;

namespace ON.Mercury.Service.Controllers
{
    [ApiController]
    [Route("/api/v1/mercury/{Controller}")]
    public class AuthController :  ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly PostgresContext _postgres;         // TODO: Replace with new MemberService
        
        public AuthController(ILogger<AuthController> logger, PostgresContext postgres)
        {
            _logger = logger;
            _postgres = postgres;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var user = ONUserHelper.ParseUser(HttpContext);
            if (user is not null)
            {
                var member = await _postgres.Members.Where(m => m.Id == user.Id.ToString()).FirstOrDefaultAsync();
                if (member is null)
                {
                    var username = user.ToClaims().Where(c => c.Type == "Display").Select(c => c.Value).FirstOrDefault();
                    var newMember = new MemberEntity()
                    {
                        Id = user.Id.ToString(),
                        Username = username
                    };
                    await _postgres.Members.AddAsync(newMember);
                    await _postgres.SaveChangesAsync();
                    return Ok(newMember);
                }

                return Ok(member);
            }

            return BadRequest("False");
        }
    }
}
