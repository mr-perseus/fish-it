using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
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
            SetAllFishingTrips();
            RefreshIsListEmpty();
            InitializeComponent();
        }

        public FishingTripsPage(string location)
        {
            SetAllFishingTrips();
            InitializeComponent();

            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public bool IsListEmpty { get; set; } = true;

        private async Task SetAllFishingTrips()
        {
            var allFishingTrips = await new FishingTripManager().GetAllFishingTrips();
            _fishingTrips = new ObservableCollection<FishingTrip>(allFishingTrips);

            if (FishingTripsListView != null)
            {
                FishingTripsListView.ItemsSource = _fishingTrips;
            }
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
            RefreshIsListEmpty();
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
            RefreshIsListEmpty();
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            SetAllFishingTrips();
            RefreshIsListEmpty();
            FishingTripsListView.EndRefresh();
        }

        private void RefreshIsListEmpty()
        {
            if (_fishingTrips != null) IsListEmpty = _fishingTrips.Count == 0;
        }
    }
}