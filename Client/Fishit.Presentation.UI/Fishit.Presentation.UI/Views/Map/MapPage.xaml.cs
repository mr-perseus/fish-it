using System.Collections.ObjectModel;
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

            MapListView.ItemsSource = _locations;
        }

        public void InitializeData()
        {
            _locations = new ObservableCollection<string> {"Zurichsee", "Bodensee", "Genfersee"};
        }
    }
}