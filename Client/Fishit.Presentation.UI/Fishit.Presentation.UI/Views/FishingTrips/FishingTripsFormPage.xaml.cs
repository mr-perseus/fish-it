using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsFormPage : ContentPage
    {
        public IList<FishingTrip.Weather> WeatherOptions
        {
            get
            {
                return Enum.GetValues(typeof(FishingTrip.Weather)).Cast<FishingTrip.Weather>().ToList();
            }
        }

        public FishingTrip.Weather SelectedWeather { get; set; }
        public FishingTripsFormPage() : this(new FishingTrip()) { }

        public FishingTripsFormPage(FishingTrip fishingTrip)
        {
            InitializeComponent();
            BindingContext = fishingTrip;
            SelectedWeather = fishingTrip.PredominantWeather;
        }

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
    }
}