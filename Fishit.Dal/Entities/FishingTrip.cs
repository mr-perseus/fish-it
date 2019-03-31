using System;

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
        };

        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public Weather PredominantWeather { get; set; }
        public double TemperatureCelcius { get; set; }
    }
}