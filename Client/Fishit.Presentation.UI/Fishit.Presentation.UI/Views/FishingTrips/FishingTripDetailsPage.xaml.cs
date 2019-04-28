using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            Response<FishingTrip> response = await new FishingTripManager().DeleteFishingTrip(FishingTrip);

            InformUserHelper<FishingTrip> informer = new InformUserHelper<FishingTrip>(response, this, "Fishing trip has been deleted successfully!");
            informer.InformUserOfResponse();
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }
    }
}