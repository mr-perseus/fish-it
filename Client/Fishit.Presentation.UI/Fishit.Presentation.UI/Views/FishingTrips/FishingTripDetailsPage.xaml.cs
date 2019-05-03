using System;
using System.Collections.Generic;
using System.Linq;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripDetailsPage : ContentPage, IPageBase
    {
        public FishingTripDetailsPage(FishingTripsPage caller, FishingTrip fishingTrip)
        {
            Caller = caller;
            RefreshData(fishingTrip);
            InitializeComponent();
        }

        public FishingTrip FishingTrip { get; set; }

        public int NumberOfCatches { get; set; }
        public string Name { get; set; }
        public FishingTripsPage Caller { get; set; }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private async void ViewCatches_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CatchesListPage(FishingTrip));
        }

        public void RefreshData(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            List<Catch> catchArrayToList = fishingTrip.Catches.ToList();
            NumberOfCatches = catchArrayToList.Count;
            Caller.ReloadFishingTrips();
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage(this, FishingTrip));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            Response<FishingTrip> response = await new FishingTripManager().DeleteFishingTrip(FishingTrip);

            InformUserHelper<FishingTrip> informer =
                new InformUserHelper<FishingTrip>(response, this);
            informer.InformUserOfResponse("Fishing trip has been deleted successfully!");

            Caller.ReloadFishingTrips();
            await Navigation.PopAsync();
        }
    }
}