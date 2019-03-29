using Fishit.Presentation.UI.Views.Account;
using Fishit.Presentation.UI.Views.FishingTrips;
using Fishit.Presentation.UI.Views.Map;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            Children.Add(new MapPage());
            Children.Add(new FishingTripsPage());
            Children.Add(new AccountPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}