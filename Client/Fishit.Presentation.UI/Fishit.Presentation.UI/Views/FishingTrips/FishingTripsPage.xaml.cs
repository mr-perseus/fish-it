using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsPage : ContentPage, IPageBase
    {
        private ObservableCollection<FishingTrip> _fishingTrips;

        public FishingTripsPage()
        {
            SetAllFishingTrips();
            InitializeComponent();
        }

        public FishingTripsPage(string location)
        {
            SetAllFishingTrips();
            InitializeComponent();

            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private async Task SetAllFishingTrips()
        {
            Response<List<FishingTrip>> response = await new FishingTripManager().GetAllFishingTrips();
            _fishingTrips = new ObservableCollection<FishingTrip>(response.Content);

            if (FishingTripsListView != null)
            {
                FishingTripsListView.ItemsSource = _fishingTrips;
            }
        }

        private async void TripsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            FishingTrip fishingTrip = e.SelectedItem as FishingTrip;
            await Navigation.PushAsync(new FishingTripDetailsPage(fishingTrip));
            FishingTripsListView.SelectedItem = null;
        }

        private async void AddTrip_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage());
            await SetAllFishingTrips();
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            Console.Write(fishingTrip);
            await Navigation.PushAsync(new FishingTripsFormPage(fishingTrip));
            await SetAllFishingTrips();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response = await manager.DeleteFishingTrip(fishingTrip);

            InformUserHelper<FishingTrip> informer =
                new InformUserHelper<FishingTrip>(response, this, "Fishing Trip has been deleted successfully!");
            informer.InformUserOfResponse();
            await SetAllFishingTrips();
        }

        private async void Handle_Refreshing(object sender, EventArgs e)
        {
            await SetAllFishingTrips();
            FishingTripsListView.EndRefresh();
        }
    }
}