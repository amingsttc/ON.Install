using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ON.Authentication;
using ON.Mercury.Service.Database;
using System.Threading.Tasks;

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
        public Task Authenticate([FromHeader] string Authentication)
        {
            var user = ONUserHelper.ParseUser(HttpContext);
            _logger.LogInformation(user.Id.ToString());
            return Task.CompletedTask;
        }
    }
}
