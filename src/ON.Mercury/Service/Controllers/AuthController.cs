using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Authentication;
using ON.Mercury.Service.Models.Auth;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ON.Mercury.Service.Database.Entities;
using ON.Mercury.Service.Database.Repositories;
using Service.Database.Entities;

namespace ON.Mercury.Service.Controllers
{
    [ApiController]
    [Route("/api/mercury/{Controller}")]
    public class AuthController :  ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly MemberRepository _members;
        
        public AuthController(ILogger<AuthController> logger, MemberRepository members)
        {
            _logger = logger;
            _members = members;
        }

        [HttpGet]
        public async Task<IActionResult> Authenticate()
        {
            var user = ONUserHelper.ParseUser(HttpContext);
            var res = new AuthenticateResponse()
            {
                IsSuccess = true,
                Errors = "",
            };
            if (user is not null)
            {
                var username = user.ToClaims().Where(c => c.Type == "Display").Select(c => c.Value).FirstOrDefault();
                var profileStr = user.ToClaims().Where(c => c.Type == "MercuryProfile").Select(c => c.Value).FirstOrDefault();
                if (string.IsNullOrEmpty(profileStr))
                {
                    var newMember = await _members.CreateMember(user.Id.ToString(), username);
                    res.Member = newMember;
                }
                else
                {
                    //res.Member = JsonConvert.DeserializeObject<Member>(profileStr);
                    res.Member = await _members.GetMember(user.Id.ToString());
                }
                
                return Ok(res);
            }
            
            return BadRequest(new AuthenticateResponse()
            {
                IsSuccess = false,
                Errors = "No User Data"
            });
        }

        [HttpGet("members")]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _members.GetMembers();
            return Ok(members);
        }
    }
}
