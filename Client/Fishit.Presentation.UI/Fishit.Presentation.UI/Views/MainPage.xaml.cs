using System.Collections.Generic;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views.Account;
using Fishit.Presentation.UI.Views.FishingTrips;
using Fishit.Presentation.UI.Views.Map;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace Fishit.Presentation.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage(List<FishingTrip> fishingTrips)
        {
            InitializeComponent();
            Children.Add(new MapPage());
            Children.Add(new FishingTripsPage(fishingTrips));
            Children.Add(new AccountPage());

            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}