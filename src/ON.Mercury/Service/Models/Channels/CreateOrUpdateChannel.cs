using System.Collections.Generic;
using Newtonsoft.Json;
using ON.Fragments.Mercury;
using Role = Service.Database.Entities.Role;

namespace ON.Mercury.Service.Models.Channels
{
    public class CreateOrUpdateChannel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("roles")]
        public IEnumerable<Role> Roles { get; set; }
    }
}
