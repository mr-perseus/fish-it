using System;
using Newtonsoft.Json;

namespace Fishit.Dal.Entities
{
    public class Catch
    {
        [JsonProperty("_id")] public string Id { get; set; } = "0";

        public FishType FishType { get; set; }
        public DateTime DateTime { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
    }
}