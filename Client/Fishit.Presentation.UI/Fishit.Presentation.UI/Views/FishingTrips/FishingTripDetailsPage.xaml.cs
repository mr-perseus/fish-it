using System;
using System.Collections.Generic;
using System.Linq;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripDetailsPage : ContentPage
    {
        public FishingTripDetailsPage(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            List<Catch> catchArrayToList = fishingTrip.Catches.ToList();
            NumberOfCatches = catchArrayToList.Count;
            InitializeComponent();
        }

        public FishingTrip FishingTrip { get; set; }

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

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            bool wasSuccessful = await new FishingTripManager().DeleteFishingTrip(FishingTrip.Id);
            if (wasSuccessful)
                await DisplayAlert("Fishing Trip", "Deleted Successfully", "Ok");
            else
                await DisplayAlert("Fishing Trip", "Something went wrong, please try again", "Ok");
        }
    }
}