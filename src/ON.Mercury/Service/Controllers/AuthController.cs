using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Models.Auth;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Controllers
{
    [ApiController]
    [Route("/api/v1/mercury/{Controller}")]
    public class AuthController :  ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly PostgresContext _postgres;         // TODO: Replace with new MemberService
        private readonly ONUserHelper _userHelper;
        
        public AuthController(ILogger<AuthController> logger, PostgresContext postgres, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _postgres = postgres;
            _userHelper = new ONUserHelper(contextAccessor);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            var user = _userHelper.MyUser;
            
            HttpContext.Response.Redirect("http://localhost:8015/api/v1/mercury/auth/login");
            return Ok();
        }

        [HttpPost("login")]
        public async Task Login()
        {
            _logger.LogInformation("HIT");
        }
    }
}
