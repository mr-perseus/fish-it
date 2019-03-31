using System.Collections.ObjectModel;
using Fishit.Presentation.UI.Models;
using Fishit.Presentation.UI.Views.FishingTrips;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private ObservableCollection<Location> _locations;
        public MapPage()
        {
            InitializeComponent();

            _locations = GetLocations();
            MapListView.ItemsSource = _locations;
        }

        private ObservableCollection<Location> GetLocations()
        {
            return new ObservableCollection<Location>
            {
                new Location {Name =  "Zurichsee"},
                new Location {Name = "Bodensee"},
                new Location {Name = "Genfersee"}
            };
        }

        private async void MapListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            Location location = e.SelectedItem as Location;
            await Navigation.PushAsync(new FishingTripsPage(location));
            MapListView.SelectedItem = null;
        }
    }
}