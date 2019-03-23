using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Fishit.Presentation.UI.Models;

namespace Fishit.Presentation.UI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FishingTrips : ContentPage
	{
        private ObservableCollection<FishingTrip> _fishingTrips;

        ObservableCollection<FishingTrip> getFishingTrips()
        {
            return new ObservableCollection<FishingTrip>
            {
                new FishingTrip { Name="Fishing Trip #1", Info="This was an amazing Fishing Trip", Location="Zurichsee" },
                new FishingTrip { Name="Fishing Trip #2", Info="The worst Trip ever.", Location="Bodensee" },
                new FishingTrip { Name="Fishing Trip #3", Info="Maybe next Time, who knows.", Location="Zurichsee" },
                new FishingTrip { Name="Fishing Trip #4", Info="Great Location", Location="Bodensee" },
                new FishingTrip { Name="Fishing Trip #5", Info="Uhh the french fishes are crazy small.", Location="Genfersee" },
                new FishingTrip { Name="Fishing Trip #6", Info="Wasn't too bad here.", Location="Bodensee" },
                new FishingTrip { Name="Fishing Trip #7", Info="Really shouldn't go here anymore.", Location="Genfersee" }
            };
        }

		public FishingTrips ()
		{
			InitializeComponent ();

            _fishingTrips = getFishingTrips();

            tripsListView.ItemsSource = _fishingTrips;
		}

        private void TripsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // allow only tapping
            tripsListView.SelectedItem = null;
        }
        private void TripsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // how to get the element
            var fishingTrip = e.Item as FishingTrip;
        }

        private void Edit_Clicked(object sender, System.EventArgs e)
        {
            var fishingTrip = (sender as MenuItem).CommandParameter as FishingTrip;

            DisplayAlert("Edit", fishingTrip.Name, "OK");
        }

        private void Delete_Clicked(object sender, System.EventArgs e)
        {
            var fishingTrip = (sender as MenuItem).CommandParameter as FishingTrip;
            _fishingTrips.Remove(fishingTrip);
        }

        private void Handle_Refreshing(object sender, System.EventArgs e)
        {
            _fishingTrips = getFishingTrips();
            tripsListView.ItemsSource = _fishingTrips;
            tripsListView.EndRefresh();
        }
    }
}