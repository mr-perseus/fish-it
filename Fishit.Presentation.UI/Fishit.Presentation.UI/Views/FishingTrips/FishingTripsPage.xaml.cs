using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsPage : ContentPage
    {
        private readonly ObservableCollection<FishingTrip> _fishingTrips;
        private readonly FishingTripManager manager;

        public FishingTripsPage()
        {
            InitializeComponent();

            manager = new FishingTripManager();
            _fishingTrips = new ObservableCollection<FishingTrip>(manager.GetAllFishingTrips());
            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public FishingTripsPage(string location)
        {
            InitializeComponent();

            manager = new FishingTripManager();
            _fishingTrips = new ObservableCollection<FishingTrip>(manager.GetAllFishingTrips());
            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        private ObservableCollection<FishingTrip> GetFishingTripsByLocation(string location)
        {
            return new ObservableCollection<FishingTrip>(_fishingTrips.Where(trips => trips.Location == location));
        }

        private async void TripsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            FishingTrip fishingTrip = e.SelectedItem as FishingTrip;
            await Navigation.PushAsync(new FishingTripDetailsPage(fishingTrip));
            FishingTripsListView.SelectedItem = null;
        }

        private async void AddTrip_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage());
        }

        private void Edit_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem).CommandParameter as FishingTrip;

            DisplayAlert("Edit", fishingTrip.Name, "OK");
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem).CommandParameter as FishingTrip;
            _fishingTrips.Remove(fishingTrip);
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            FishingTripsListView.ItemsSource = _fishingTrips;
            FishingTripsListView.EndRefresh();
        }
    }
}