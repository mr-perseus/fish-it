using Newtonsoft.Json;

namespace Fishit.Dal.Entities
{
    public class FishType
    {
        [JsonProperty("_id")]
        public string FishTypeId { get; set; } = "0";
        public string Name { get; set; } = "noFishtype";
        public string Description { get; set; } = "noDescription";
    }
}