using System;
using System.Threading;
using System.Threading.Tasks;
using Fishit.Common;
using Fishit.Dal.Entities;

namespace Fishit.TestEnvironment
{
    public static class TestEnvironmentHelper
    {
        private const string InitializationError = "Error while re-initializing database entries.";
        public static string FishingTripId { get; private set; }
        private static bool _firstTestInExecution = true;
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

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
                        FishTypeId = "05cb34f29500b0509f4244306",
                        Name = "Tuna",
                        Description = "Meeresfisch, mit Sonde gefangen"
                    },
                    DateTime = new DateTime(2019, 04, 16, 11, 25, 00),
                    Length = 50,
                    Weight = 100,
                    CatchId = "0",
                }
            }
        };

        static TestEnvironmentHelper()
        {
            AppDomain.CurrentDomain.ProcessExit +=
                TestEnvironmentHelperDestructor;
        }

        public static async Task InitializeTestDataAndGetId()
        {
            try
            {
                if (_firstTestInExecution)
                {
                    _firstTestInExecution = false;

                    await SemaphoreSlim.WaitAsync();
                    await new FishingTripDao().CreateFishingTrip(TestFishingTrip);
                    SemaphoreSlim.Release();
                    FishingTripId = "ID-TODO"; // TODO
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(InitializationError, exception);
            }
        }

        private static async void TestEnvironmentHelperDestructor(object sender, EventArgs e)
        {
            await new FishingTripDao().DeleteFishingTrip(FishingTripId);
        }
    }
}