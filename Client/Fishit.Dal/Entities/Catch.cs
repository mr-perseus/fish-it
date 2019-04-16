using System;

namespace Fishit.Dal.Entities
{
    public class Catch
    {
        public string Id { get; set; }
        public Fishtype FishType { get; set; }
        public DateTime DateTime { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
    }
}