using System;
using System.Globalization;

namespace Fishit.Dal.Entities
{
    public class Catch
    {
        public string Id { get; set; } = "0";
        public FishType FishType { get; set; }
        public DateTime DateTime { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }

        public override string ToString()
        {
            return base.ToString() + "; " +
                   nameof(Id) + "; " + Id + "; " +
                   nameof(FishType) + "; " + (FishType != null ? FishType.ToString() : "null") + "; " +
                   nameof(DateTime) + "; " + DateTime.ToString(CultureInfo.CurrentCulture) + "; " +
                   nameof(Length) + "; " + Length + "; " +
                   nameof(Weight) + "; " + Weight + "; ";
        }
    }
}