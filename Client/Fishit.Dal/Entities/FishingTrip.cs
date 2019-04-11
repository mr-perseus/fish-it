using System;
using System.Collections.Generic;
using System.Globalization;

namespace Fishit.Dal.Entities
{
    public class FishingTrip
    {
        public enum Weather
        {
            Sunny,
            Overcast,
            Raining,
            Snowing,
            Hailing
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public Weather PredominantWeather { get; set; }
        public double Temperature { get; set; }
        public List<Catch> Catches { get; set; }


        public override string ToString()
        {
            return base.ToString() + "; " +
                   nameof(Id) + "; " + Id + "; " +
                   nameof(Name) + "; " + Name + "; " +
                   nameof(Location) + "; " + Location + "; " +
                   nameof(DateTime) + "; " + DateTime.ToString(CultureInfo.CurrentCulture) + "; " +
                   nameof(PredominantWeather) + "; " + PredominantWeather + "; " +
                   nameof(Temperature) + "; " + Temperature + "; " +
                   nameof(Catches) + "; " + (Catches != null ? Catches.Count.ToString() : "null");
        }
    }
}