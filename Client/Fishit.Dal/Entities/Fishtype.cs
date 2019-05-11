using Newtonsoft.Json;

namespace Fishit.Dal.Entities
{
    public class FishType
    {
        [JsonProperty("_id")]  public string Id { get; set; } = "0";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";

        public override string ToString()
        {
            return base.ToString() + "; " +
                   nameof(Id) + "; " + Id + "; " +
                   nameof(Name) + "; " + Name + "; " +
                   nameof(Description) + "; " + Description + "; ";
        }
    }
}