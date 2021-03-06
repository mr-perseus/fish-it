﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsPage : IPageBase
    {
        private ObservableCollection<FishingTrip> _fishingTrips;

        public FishingTripsPage(List<FishingTrip> fishingTrips)
        {
            InitializeComponent();

            _fishingTrips = new ObservableCollection<FishingTrip>(fishingTrips);
            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        public async Task ReloadFishingTrips()
        {
            Response<List<FishingTrip>> response = await new FishingTripManager().GetAllFishingTrips();
            if ((int) response.StatusCode < 400)
            {
                _fishingTrips = new ObservableCollection<FishingTrip>(response.Content);
            }

            InformUserHelper<List<FishingTrip>> informer =
                new InformUserHelper<List<FishingTrip>>(response, this);

            informer.InformUserOfResponse();

            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        private async void TripsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            FishingTrip fishingTrip = e.SelectedItem as FishingTrip;
            await Navigation.PushAsync(new FishingTripDetailsPage(this, fishingTrip));
            FishingTripsListView.SelectedItem = null;
        }

        private async void AddTrip_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage(this));
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            await Navigation.PushAsync(new FishingTripsFormPage(this, fishingTrip));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response = await manager.DeleteFishingTrip(fishingTrip);

            InformUserHelper<FishingTrip> informer =
                new InformUserHelper<FishingTrip>(response, this);
            informer.InformUserOfResponse();
            await ReloadFishingTrips();
        }

        private async void Handle_Refreshing(object sender, EventArgs e)
        {
            await ReloadFishingTrips();
            FishingTripsListView.EndRefresh();
        }
    }
}