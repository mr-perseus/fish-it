using System;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripDetailsPage : ContentPage
    {
        public int NumberOfCatches { get; set; }
        private FishingTrip _fishingTrip;

        public FishingTripDetailsPage(FishingTrip fishingTrip)
        {
            _fishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            NumberOfCatches = fishingTrip.Catches.Count;
            InitializeComponent();
        }

        private async void ViewCatches_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CatchesListPage(_fishingTrip));
        }
    }
}