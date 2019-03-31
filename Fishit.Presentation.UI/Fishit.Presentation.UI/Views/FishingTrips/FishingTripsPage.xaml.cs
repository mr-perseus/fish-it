using System;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Text.Style;
using Fishit.Presentation.UI.Models;
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
            InitializeFishingTrips();
            InitializeComponent();
            
            FishingTripsListView.ItemsSource = _fishingTrips;
        }
        public FishingTripsPage(Location location)
        {
            InitializeFishingTrips();
            InitializeComponent();

            FishingTripsListView.ItemsSource = GetFishingTripsByLocation(location);
        }

        private void InitializeFishingTrips()
        {
            _fishingTrips = new ObservableCollection<FishingTrip>
            {
                new FishingTrip
                    {Name = "Fishing Trip #1", Info = "This was an amazing Fishing Trip", Location = "Zurichsee"},
                new FishingTrip {Name = "Fishing Trip #2", Info = "The worst Trip ever.", Location = "Bodensee"},
                new FishingTrip
                    {Name = "Fishing Trip #3", Info = "Maybe next Time, who knows.", Location = "Zurichsee"},
                new FishingTrip {Name = "Fishing Trip #4", Info = "Great Location", Location = "Bodensee"},
                new FishingTrip
                    {Name = "Fishing Trip #5", Info = "Uhh the french fishes are crazy small.", Location = "Genfersee"},
                new FishingTrip {Name = "Fishing Trip #6", Info = "Wasn't too bad here.", Location = "Bodensee"},
                new FishingTrip
                    {Name = "Fishing Trip #7", Info = "Really shouldn't go here anymore.", Location = "Genfersee"}
            };
        }

        private ObservableCollection<FishingTrip> GetFishingTripsByLocation(Location location)
        {
            return new ObservableCollection<FishingTrip>(_fishingTrips.Where(trips => trips.Location == location.Name));
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