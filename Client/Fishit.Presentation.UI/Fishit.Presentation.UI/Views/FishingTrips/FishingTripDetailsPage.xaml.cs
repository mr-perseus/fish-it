﻿using System;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripDetailsPage : IPageBase
    {
        public FishingTripDetailsPage(FishingTripsPage caller, FishingTrip fishingTrip)
        {
            Caller = caller;
            SetBindingContext(fishingTrip);
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

        private void SetBindingContext(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            NumberOfCatches = fishingTrip.Catches.Count;
        }

        public async Task RefreshData(FishingTrip fishingTrip)
        {
            SetBindingContext(fishingTrip);
            await Caller.ReloadFishingTrips();
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
            informer.InformUserOfResponse();

            await Caller.ReloadFishingTrips();
            await Navigation.PopAsync();
        }
    }
}