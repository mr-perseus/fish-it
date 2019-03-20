using System;
using Xunit;

namespace Fishit.BusinessLayer.Testing
{
    public class FishingTripManagerTest
    {
        [Fact]
        public void Test1()
        {
            FishingTripManager fishingTripManager = new FishingTripManager();

            // TODO

            Assert.Equal(typeof(FishingTripManager), fishingTripManager.GetType());
        }
    }
}
