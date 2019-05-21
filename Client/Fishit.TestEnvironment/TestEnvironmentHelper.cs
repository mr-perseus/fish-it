using System;
using System.Threading.Tasks;
using Fishit.Common;
using Fishit.Dal.Entities;
using Fishit.Logging;

namespace Fishit.TestEnvironment
{
    public static class TestEnvironmentHelper
    {
        private const string InitializationError = "Error while re-initializing database entries.";

        private static readonly ILogger _logger = LogManager.GetLogger(nameof(TestEnvironmentHelper));
        // private static volatile bool _initialized;
        // private static string _fishingTripId;

        private static readonly FishingTrip TestFishingTrip = new FishingTrip
        {
            Location = "Letzte",
            DateTime = new DateTime(2019, 04, 16),
            Description = "Neu POST Versuch",
            PredominantWeather = FishingTrip.Weather.Sunny,
            Temperature = 12.5,
            Catches =
            {
                new Catch
                {
                    FishType = new FishType
                    {
                        Id = "05cb34f29500b0509f4244306",
                        Name = "Tuna",
                        Description = "Meeresfisch, mit Sonde gefangen"
                    },
                    DateTime = new DateTime(2019, 04, 16, 11, 25, 00),
                    Length = 50,
                    Weight = 100,
                    Id = "5cb34f68500b0509f4244307"
                }
            }
        };

        public static async Task InitTestData(Func<FishingTrip, Task> function)
        {
            try
            {
                // TODO Jan
                /*if (!_initialized)
                {
                    _initialized = true;
                    _fishingTripId = await new FishingTripDao().CreateFishingTrip(TestFishingTrip);
                }

                while (_fishingTripId == "0")
                {
                }

                await function(_fishingTripId);*/

                Response<FishingTrip> response = await new Dao<FishingTrip>().CreateItem(TestFishingTrip);
                await function(response.Content);

                await new Dao<FishingTrip>().DeleteItem(response.Content);
            }
            catch (Exception exception)
            {
                throw new ApplicationException(InitializationError, exception);
            }
        }
    }
}