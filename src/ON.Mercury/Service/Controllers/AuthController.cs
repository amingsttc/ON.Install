using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Authentication;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Models.Auth;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ON.Mercury.Service.Database.Repositories;

namespace ON.Mercury.Service.Controllers
{
    [ApiController]
    [Route("/api/mercury/{Controller}")]
    public class AuthController :  ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly MemberRepository _members;
        //private readonly ServiceNameHelper 
        
        public AuthController(ILogger<AuthController> logger, MemberRepository members)
        {
            _logger = logger;
            _members = members;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var c = ControllerContext.HttpContext.Request;
            _logger.LogInformation(JsonConvert.SerializeObject(c));
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
