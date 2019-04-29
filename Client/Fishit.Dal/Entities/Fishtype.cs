using Newtonsoft.Json;

namespace Fishit.Dal.Entities
{
    public class FishType
    {
        [JsonProperty("_id")] public string Id { get; set; } = "0";

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
    }
}