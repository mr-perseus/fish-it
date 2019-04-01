using System;
using System.Collections.ObjectModel;
using System.Linq;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsPage : ContentPage
    {
        private ObservableCollection<FishingTrip> _fishingTrips;

        public FishingTripsPage()
        {
            InitializeData();
            InitializeComponent();

            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public FishingTripsPage(string location)
        {
            InitializeData();
            InitializeComponent();
            _fishingTrips =
                new ObservableCollection<FishingTrip>(_fishingTrips.Where(trip => trip.Location == location));

            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public void InitializeData()
        {
            _fishingTrips = new ObservableCollection<FishingTrip>
            {
                new FishingTrip {Name = "Fishing Trip #1", Location = "Zurichsee"},
                new FishingTrip {Name = "Fishing Trip #2", Location = "Bodensee"},
                new FishingTrip {Name = "Fishing Trip #3", Location = "Zurichsee"},
                new FishingTrip {Name = "Fishing Trip #4", Location = "Bodensee"},
                new FishingTrip {Name = "Fishing Trip #5", Location = "Genfersee"},
                new FishingTrip {Name = "Fishing Trip #6", Location = "Bodensee"},
                new FishingTrip {Name = "Fishing Trip #7", Location = "Genfersee"}
            };
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

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            Console.Write(fishingTrip);
            await Navigation.PushAsync(new FishingTripsFormPage(fishingTrip));
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            _fishingTrips.Remove(fishingTrip);
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            FishingTripsListView.ItemsSource = _fishingTrips;
            FishingTripsListView.EndRefresh();
        }
    }
}