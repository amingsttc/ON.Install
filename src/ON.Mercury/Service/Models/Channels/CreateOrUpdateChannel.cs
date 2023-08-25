using Newtonsoft.Json;

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
    }
}
