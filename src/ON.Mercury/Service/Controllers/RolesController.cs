using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Authentication;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using ON.Mercury.Service.Database.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ON.Mercury.Service.Controllers
{
    [ApiController]
    [Route("/api/mercury/{Controller}")]
    public class RolesController : ControllerBase
    {
        // TODO: Look Into CQRS
        private readonly ILogger<RolesController> _logger;
        private readonly PostgresContext _postgres;
        private readonly RoleRepository _roles;
        
        public RolesController(ILogger<RolesController> logger, PostgresContext postgres, RoleRepository roles)
        {
            _logger = logger;
            _postgres = postgres;
            _roles = roles;
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _roles.GetRolesAsync(cancellationToken);
            if (roles is null)
            {
                return BadRequest("Null response from role repository");
            }

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequest request, CancellationToken cancellationToken = default)
        {
            var newRoleId = await _roles.CreateRoleAsync(request.Name, request.Permissions, request.Hierarchy, cancellationToken);
            if (string.IsNullOrEmpty(newRoleId)) return BadRequest("Role failed to create");
            return Ok($"Role Created: {newRoleId}");
        }

        [HttpPut("{roleId}")]
        public async Task<IActionResult> UpdateRoleAsync(string roleId, [FromBody] UpdateRoleRequest request, CancellationToken cancellationToken = default)
        {
            var updatedRole = await _roles.UpdateRoleAsync(roleId, request.Name, request.Permissions, request.Hierarchy, cancellationToken);
            if (updatedRole is null) return BadRequest("Role failed to update");
            return Ok(updatedRole);
        }

        [HttpDelete("{roleId}")]
        public async Task<IActionResult> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default)
        {
            var deletedRoleId = await _roles.DeleteRoleAsync(roleId, cancellationToken);
            if (string.IsNullOrEmpty(deletedRoleId)) return BadRequest("Role failed to delete");
            return Ok($"Role Created: {deletedRoleId}");
        }
    }
}
