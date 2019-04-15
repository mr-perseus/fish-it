﻿using System;
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
        public int NumberOfCatches { get; set; }
        private FishingTrip _fishingTrip;
        public string Name { get; set; }

        public FishingTripDetailsPage(FishingTrip fishingTrip)
        {
            _fishingTrip = fishingTrip;
            BindingContext = fishingTrip;
            List<Catch> catchArrayToList = fishingTrip.Catches.ToList();
            NumberOfCatches = catchArrayToList.Count;
            InitializeComponent();
        }

        private async void ViewCatches_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CatchesListPage(_fishingTrip));
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage(_fishingTrip));
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            //Call manager to remove
        }
    }
}