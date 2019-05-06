using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage, IPageBase
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
            await LoadFishingTrips();
            await Navigation.PushAsync(new MainPage(_fishingTrips));
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
    }
}