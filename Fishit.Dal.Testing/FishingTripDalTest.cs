using System;
using System.Linq;
using Xunit;
using Fishit.Dal;
using Fishit.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Fishit.Dal.Testing
{
    public class FishingTripDalTest
    {
        [Fact]
        public void TestAddFishingTrip()
        {
            using (FishitContext context = new FishitContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            String name = "Meier";
            FishingTrip fishingTrip = new FishingTrip
            {
                Name = name
            };

            using (FishitContext context = new FishitContext())
            {
                context.Entry(fishingTrip).State = EntityState.Added;
                context.SaveChanges();
            }

            int id = 1;
            FishingTrip fishingTripFromDb;
            using (FishitContext context = new FishitContext())
            {
                fishingTripFromDb = context.FishingTrips.FirstOrDefault(k => k.Id == id);
            }

            Assert.NotNull(fishingTripFromDb);
            Assert.Equal(name, fishingTripFromDb.Name);
        }
    }
}
