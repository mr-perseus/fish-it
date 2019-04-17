namespace Fishit.BusinessLayer.Testing
{
    public class FishingTripManagerTest
    {
        private FishingTripManager _fishingTripManager;

        private FishingTripManager FishingTripManager =>
            _fishingTripManager ?? (_fishingTripManager = new FishingTripManager());
    }
}