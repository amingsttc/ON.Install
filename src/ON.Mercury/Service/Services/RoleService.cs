using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using ON.Mercury.Service.Database;
using Service.Database.Entities;

namespace ON.Mercury.Service.Services;

[Authorize]
public class RoleService : RoleInterface.RoleInterfaceBase
{
    private readonly ILogger<RoleService> _logger;
    private readonly PostgresContext _postgres;

    public RoleService(ILogger<RoleService> logger, PostgresContext postgres)
    {
        _logger = logger;
        _postgres = postgres;
    }

    public override async Task<CreateRoleResponse> CreateRole(CreateRoleRequest request, ServerCallContext context)
    {
        try
        {
            var newRole = new RoleEntity(request.Name, request.Hierarchy) { };

            var json = JsonConvert.SerializeObject(request.Permissions);
            newRole.Permissions = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);

            await _postgres.Roles.AddAsync(newRole);
            await _postgres.SaveChangesAsync();
            return new CreateRoleResponse()
            {
                IsSuccess = true,
                Error = "",
                Role = newRole.ToPb()
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(e));
            return new CreateRoleResponse()
            {
                IsSuccess = false
            };
        }
    }

    public override async Task<GetRolesResponse> GetRoles(GetRolesRequest request, ServerCallContext context)
    {
        try
        {
            var roles = await _postgres.Roles.ToListAsync();
            var response = new GetRolesResponse()
            {
                IsSuccess = true,
                Error = ""
            };

            if (roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    var proto = role.ToPb();
                    response.Roles.Add(proto);
                }
            }

            return response;
        }
        catch (Exception e)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(e));
            return new GetRolesResponse()
            {
                IsSuccess = false,
            };
        }
    }

    public override async Task<DeleteRoleResponse> DeleteRole(DeleteRoleRequest request, ServerCallContext context)
    {
        try
        {
            var foundRole = await _postgres.Roles.Where(e => e.Id == request.RoleId).FirstOrDefaultAsync();
            if (foundRole is null)
            {
                return new DeleteRoleResponse()
                {
                    IsSuccess = false,
                    Error = "Role Not Found"
                };
            }

            _postgres.Roles.Remove(foundRole);
            await _postgres.SaveChangesAsync();
            return new DeleteRoleResponse()
            {
                IsSuccess = true,
                Error = "",
            };
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return new DeleteRoleResponse()
            {
                IsSuccess = false
            };
        }
    }

    public override async Task<UpdateRoleResponse> UpdateRole(UpdateRoleRequest request, ServerCallContext context)
    {
        try
        {
            var foundRole = await _postgres.Roles.Where(e => e.Id == request.RoleId).FirstOrDefaultAsync();
            if (foundRole is null)
            {
                return new UpdateRoleResponse()
                {
                    IsSuccess = false,
                    Error = "Role not found"
                };
            }

            foundRole.Name = request.Name;
            
            var json = JsonConvert.SerializeObject(request.Permissions);
            foundRole.Permissions = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
            foundRole.Hierarchy = request.Hierarchy;

            _postgres.Roles.Update(foundRole);
            await _postgres.SaveChangesAsync();

            return new UpdateRoleResponse()
            {
                IsSuccess = true,
                Error = "",
                UpdatedRole = foundRole.ToPb()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new UpdateRoleResponse()
            {
                IsSuccess = false
            };
        }
    }
}