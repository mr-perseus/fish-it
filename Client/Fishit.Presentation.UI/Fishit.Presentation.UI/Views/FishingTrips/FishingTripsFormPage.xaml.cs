using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsFormPage : IPageBase
    {
        public FishingTripsFormPage(FishingTripsPage caller) : this(caller, new FishingTrip())
        {
        }

        public FishingTripsFormPage(object caller, FishingTrip fishingTrip)
        {
            SetCaller(caller);
            SetBindingContext(fishingTrip);
            InitializeComponent();
        }

        public IEnumerable<FishingTrip.Weather> WeatherOptions => GetWeatherOptions();

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        private FishingTrip FishingTrip { get; set; }
        public bool IsEdit { get; set; }
        public FishingTrip.Weather SelectedWeather { get; set; }
        public FishingTripDetailsPage CallerFishingTripDetailsPage { get; set; }
        public FishingTripsPage CallerFishingTripsPage { get; set; }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private void SetCaller(object caller)
        {
            if (caller.GetType() == typeof(FishingTripsPage))
            {
                CallerFishingTripsPage = (FishingTripsPage) caller;
            }
            else if (caller.GetType() == typeof(FishingTripDetailsPage))
            {
                CallerFishingTripDetailsPage = (FishingTripDetailsPage) caller;
            }
        }

        private async Task SaveFishingTrip()
        {
            FishingTripManager manager = new FishingTripManager();
            FishingTrip.PredominantWeather = SelectedWeather;
            Response<FishingTrip> response;
            if (IsEdit)
            {
                response = await manager.UpdateFishingTrip(FishingTrip);
            }
            else
            {
                response = await manager.CreateFishingTrip(FishingTrip);
            }

            InformUserHelper<FishingTrip> informer =
                new InformUserHelper<FishingTrip>(response, this);
            informer.InformUserOfResponse();

            if (CallerFishingTripsPage != null)
            {
                await CallerFishingTripsPage.ReloadFishingTrips();
            }

            if (CallerFishingTripDetailsPage != null)
            {
                await CallerFishingTripDetailsPage.RefreshData(response.Content);
            }
        }

        private IEnumerable<FishingTrip.Weather> GetWeatherOptions()
        {
            return Enum.GetValues(typeof(FishingTrip.Weather)).Cast<FishingTrip.Weather>().ToList();
        }

        private async void CancelForm_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveFishingTrip_OnClicked(object sender, EventArgs e)
        {
            await SaveFishingTrip();
            await Navigation.PopAsync();
        }

        private async void ViewCatches_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CatchesListPage(FishingTrip));
        }

        public void Refresh_DateTime(object sender, EventArgs e)
        {
            FishingTrip.DateTime = new DateTime(
                Date.Year,
                Date.Month,
                Date.Day,
                Time.Hours,
                Time.Minutes,
                0);
        }

        private void SetBindingContext(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            SelectedWeather = fishingTrip.PredominantWeather;
            if (fishingTrip.Id.Equals("0"))
            {
                FishingTrip.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }
            else
            {
                IsEdit = true;
            }

            Date = FishingTrip.DateTime.Date;
            Time = FishingTrip.DateTime.TimeOfDay;
        }
    }
}