using System;

namespace Fishit.Dal.Entities
{
    public class Catch
    {
        public Fishtype Fishtype { get; set; }
        public DateTime DateTime { get; set; }
        public double SizeInMeters { get; set; }
    }
}