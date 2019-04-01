using System.Collections.ObjectModel;
using Fishit.BusinessLayer;
using Fishit.Presentation.UI.Views.FishingTrips;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        private ObservableCollection<string> _locations;

        public MapPage()
        {
            InitializeData();
            InitializeComponent();

            //FishingTripManager manager = new FishingTripManager();
            //_locations = new ObservableCollection<string>(manager.GetAllLocations());
            MapListView.ItemsSource = _locations;
        }

        public void InitializeData()
        {
            _locations = new ObservableCollection<string>() {"Zurichsee", "Bodensee", "Genfersee"};
        }

        private async void MapListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            string location = (string) e.SelectedItem;
            if (location != null) await Navigation.PushAsync(new FishingTripsPage(location));
            MapListView.SelectedItem = null;
        }
    }
}