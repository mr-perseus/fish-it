using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsFormPage : ContentPage
    {
        public FishingTripsFormPage() : this(new FishingTrip())
        {
        }

        public FishingTripsFormPage(FishingTrip fishingTrip)
        {
            SetBindingContext(fishingTrip);
            InitializeComponent();
        }

        public IList<FishingTrip.Weather> WeatherOptions =>
            Enum.GetValues(typeof(FishingTrip.Weather)).Cast<FishingTrip.Weather>().ToList();

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        private FishingTrip FishingTrip { get; set; }
        public bool IsEdit { get; set; }
        public FishingTrip.Weather SelectedWeather { get; set; }

        private async Task SaveFishingTrip()
        {
            bool wasSuccessful;
            FishingTripManager manager = new FishingTripManager();
            FishingTrip.PredominantWeather = SelectedWeather;
            if (IsEdit)
                wasSuccessful = await manager.UpdateFishingTrip(FishingTrip);
            else
                wasSuccessful = !string.IsNullOrEmpty(await manager.CreateFishingTrip(FishingTrip));

            if (wasSuccessful)
                await DisplayAlert("Fishing Trip", "Saved Successfully", "Ok");
            else
                await DisplayAlert("Fishing Trip", "Something went wrong, please try again", "Ok");
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
            if (!fishingTrip.Id.Equals("0"))
                IsEdit = true;
            else
                fishingTrip.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, DateTime.Now.Minute, 0);

            Date = fishingTrip.DateTime.Date;
            Time = fishingTrip.DateTime.TimeOfDay;
        }
    }
}