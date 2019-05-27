using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.Map
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage
    {
        public MapPage(Position currentPosition)
        {
            InitializeComponent();
            InitializeMap(currentPosition);
        }

        private void InitializeMap(Position position)
        {
            Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map(
                MapSpan.FromCenterAndRadius(
                    position, Distance.FromKilometers(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                MapType = MapType.Satellite,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            MapStackLayout.Children.Add(map);
        }
    }
}