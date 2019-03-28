using System.Linq;
using Fishit.Dal.Entities;
using Fishit.TestEnvironment;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fishit.Dal.Testing
{
    public class FishingTripDalTest : TestBase
    {
        [Fact]
        public void TestAddFishingTrip()
        {
            const string name = "Meier";
            FishingTrip fishingTrip = new FishingTrip
            {
                Name = name
            };

            using (FishitContext context = new FishitContext())
            {
                context.Entry(fishingTrip).State = EntityState.Added;
                context.SaveChanges();
            }

            const int id = 5;
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