﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Fishit.Dal.Entities
{
    public class FishingTrip
    {
        public enum Weather
        {
            Sunny = 1,
            Overcast,
            Raining,
            Snowing,
            Hailing
        }

        [JsonProperty("_id")] public string Id { get; set; } = "0";
        public string Location { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; } = "";
        public Weather PredominantWeather { get; set; }
        public double Temperature { get; set; }
        public IList<Catch> Catches { get; set; } = new List<Catch>();

        public override string ToString()
        {
            return base.ToString() + "; " +
                   nameof(Id) + "; " + Id + "; " +
                   nameof(Location) + "; " + Location + "; " +
                   nameof(DateTime) + "; " + DateTime.ToString(CultureInfo.CurrentCulture) + "; " +
                   nameof(PredominantWeather) + "; " + PredominantWeather + "; " +
                   nameof(Temperature) + "; " + Temperature + "; " +
                   nameof(Catches) + "; " + (Catches != null ? Catches.Count.ToString() : "null");
        }
    }
}