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
            ReloadFishingTrips();
            InitializeComponent();
        }

        public FishingTripsPage(string location)
        {
            ReloadFishingTrips();
            InitializeComponent();
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        public async void ReloadFishingTrips()
        {
            Response<List<FishingTrip>> response = await new FishingTripManager().GetAllFishingTrips();
            _fishingTrips = new ObservableCollection<FishingTrip>(response.Content);

            InformUserHelper<List<FishingTrip>> informer = 
                new InformUserHelper<List<FishingTrip>>(response, this);

            informer.InformUserOfResponse();

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
            await Navigation.PushAsync(new FishingTripDetailsPage(this, fishingTrip));
            FishingTripsListView.SelectedItem = null;
        }

        private async void AddTrip_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage(this));
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            Console.Write(fishingTrip);
            await Navigation.PushAsync(new FishingTripsFormPage(this, fishingTrip));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response = await manager.DeleteFishingTrip(fishingTrip);

            InformUserHelper<FishingTrip> informer =
                new InformUserHelper<FishingTrip>(response, this);
            informer.InformUserOfResponse("Fishing Trip has been deleted successfully!");
            ReloadFishingTrips();
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            ReloadFishingTrips();
            FishingTripsListView.EndRefresh();
        }
    }
}