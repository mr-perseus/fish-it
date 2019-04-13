using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Java.Util;
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
            FishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            SelectedWeather = fishingTrip.PredominantWeather;
            if (fishingTrip.Id != 0)
            {
                IsEdit = true;
            }
            else
            {
                fishingTrip.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, DateTime.Now.Minute, 0);
            }

            Date = fishingTrip.DateTime.Date;
            Time = fishingTrip.DateTime.TimeOfDay;
            InitializeComponent();
        }

        public IList<FishingTrip.Weather> WeatherOptions =>
            Enum.GetValues(typeof(FishingTrip.Weather)).Cast<FishingTrip.Weather>().ToList();
        
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        private FishingTrip FishingTrip { get; }
        public bool IsEdit { get; set; }
        public FishingTrip.Weather SelectedWeather { get; set; }

        private async Task SaveFishingTrip()
        {
            await DisplayAlert("Fishing Trip", "Saved Successfully", "Ok");
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

        private async void openCatches_OnClicked(object sender, EventArgs e)
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
    }
}