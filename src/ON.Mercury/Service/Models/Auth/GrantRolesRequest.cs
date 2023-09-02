using System.Collections.Generic;

namespace ON.Mercury.Service.Models.Roles
{
    public record GrantRolesRequest(IEnumerable<string> Roles);
}
