using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ON.Fragments.Mercury;

namespace ON.Mercury.Service.Services;

public class RoleService : RoleInterface.RoleInterfaceBase
{
    private readonly ILogger<RoleService> _logger;

    public RoleService(ILogger<RoleService> logger)
    {
        _logger = logger;
    }

    public override Task<CreateRoleResponse> CreateRole(CreateRoleRequest request, ServerCallContext context)
    {
        return base.CreateRole(request, context);
    }

    public override Task<GetRolesResponse> GetRoles(GetRolesRequest request, ServerCallContext context)
    {
        return base.GetRoles(request, context);
    }

    public override Task<DeleteRoleResponse> DeleteRole(DeleteRoleRequest request, ServerCallContext context)
    {
        return base.DeleteRole(request, context);
    }

    public override Task<UpdateRoleResponse> UpdateRole(UpdateRoleRequest request, ServerCallContext context)
    {
        return base.UpdateRole(request, context);
    }
}