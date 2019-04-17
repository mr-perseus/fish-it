using System;
using System.Collections.Generic;
using System.Linq;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripDetailsPage : ContentPage
    {
        public FishingTrip FishingTrip { get; set; }

        public FishingTripDetailsPage(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            List<Catch> catchArrayToList = fishingTrip.Catches.ToList();
            NumberOfCatches = catchArrayToList.Count;
            InitializeComponent();
        }

        public int NumberOfCatches { get; set; }
        public string Name { get; set; }

        private async void ViewCatches_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CatchesListPage(FishingTrip));
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage(FishingTrip));
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            //Call manager to remove
        }
    }
}