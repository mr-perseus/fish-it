using System.Collections.Generic;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views.Account;
using Fishit.Presentation.UI.Views.FishingTrips;
using Fishit.Presentation.UI.Views.Map;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage
    {
        public MainPage(Position position, List<FishingTrip> fishingTrips)
        {
            InitializeComponent();
            Children.Add(new MapPage(position));
            Children.Add(new FishingTripsPage(fishingTrips));
            Children.Add(new AccountPage());

            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}