using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : IPageBase
    {
        private List<FishingTrip> _fishingTrips;

        public AuthPage()
        {
            _fishingTrips = new List<FishingTrip>();
            InitializeComponent();
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private async void Login_OnClicked(object sender, EventArgs e)
        {
            LoadingIndicator.IsRunning = true;
            Position position = await GetCurrentLocation();
            await LoadFishingTrips();
            await Navigation.PushAsync(new MainPage(position, _fishingTrips));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public async Task LoadFishingTrips()
        {
            Response<List<FishingTrip>> response = await new FishingTripManager().GetAllFishingTrips();
            _fishingTrips = new List<FishingTrip>(response.Content);

            InformUserHelper<List<FishingTrip>> informer =
                new InformUserHelper<List<FishingTrip>>(response, this);

            informer.InformUserOfResponse();
        }

        private async Task<Position> GetCurrentLocation()
        {
            Plugin.Geolocator.Abstractions.Position position = await CrossGeolocator.Current.GetPositionAsync();
            return new Position(position.Latitude, position.Longitude);
        }
    }
}