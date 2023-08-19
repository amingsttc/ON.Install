using System.Collections.Generic;

namespace ON.Mercury.Service.Models.Roles
{
    public class CreateRoleRequest
    {
        public string Name { get; set; }
        public Dictionary<string, bool> Permissions { get; set; }
        public int Hierarchy { get; set; }
    }
}
