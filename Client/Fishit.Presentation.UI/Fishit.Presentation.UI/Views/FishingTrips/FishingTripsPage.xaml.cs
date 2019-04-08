using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsPage : ContentPage
    {
        private ObservableCollection<FishingTrip> _fishingTrips;

        public FishingTripsPage()
        {
            InitializeData();
            InitializeComponent();

            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public FishingTripsPage(string location)
        {
            InitializeData();
            InitializeComponent();
            _fishingTrips =
                new ObservableCollection<FishingTrip>(_fishingTrips.Where(trip => trip.Location == location));

            FishingTripsListView.ItemsSource = _fishingTrips;
        }

        public void InitializeData()
        {
            Fishtype salmon = new Fishtype {Name = "Salmon", Description = "It is a fish"};
            Fishtype tuna = new Fishtype {Name = "Tuna", Description = "That is also a fish"};

            FishingTrip fishingTrip1 = new FishingTrip {DateTime = new DateTime(2018,6,15,10,20,26), Location = "Rapperswil", Description = "It was awesome!", PredominantWeather = FishingTrip.Weather.Sunny, TemperatureCelsius = 25.4};
            FishingTrip fishingTrip2 = new FishingTrip {DateTime = new DateTime(2018,9,1,11,44,34), Location = "Zürich", Description = "Beautiful day.", PredominantWeather = FishingTrip.Weather.Sunny, TemperatureCelsius = 23.1};
            FishingTrip fishingTrip3 = new FishingTrip {DateTime = new DateTime(2019,2,17,14,50,22), Location = "Genf", Description = "It was a little bit chilli.", PredominantWeather = FishingTrip.Weather.Raining, TemperatureCelsius = 16.2};
            FishingTrip fishingTrip4 = new FishingTrip {DateTime = new DateTime(2019,1,5,13,45,0), Location = "Zürich", Description = "It was freezing, but still nice.", PredominantWeather = FishingTrip.Weather.Snowing, TemperatureCelsius = -2.9};
            FishingTrip fishingTrip5 = new FishingTrip {DateTime = new DateTime(2018,8,14,4,5,2), Location = "Rapperswil", Description = "That is too hot!", PredominantWeather = FishingTrip.Weather.Sunny, TemperatureCelsius = 35.0};

            fishingTrip1.Catches = new List<Catch>
            {
                new Catch {Fishtype = salmon, DateTime = new DateTime(2018, 6, 15, 12, 25, 6), SizeInMeters = 0.8},
                new Catch {Fishtype = salmon, DateTime = new DateTime(2018, 6, 15, 13, 45, 55), SizeInMeters = 0.95},
                new Catch {Fishtype = tuna, DateTime = new DateTime(2018, 6, 15, 16, 2, 7), SizeInMeters = 1.2},
            };

            fishingTrip2.Catches = new List<Catch>
            {
                new Catch {Fishtype = salmon, DateTime = new DateTime(2018,9,1,12,4,37), SizeInMeters = 0.4},
                new Catch {Fishtype = salmon, DateTime = new DateTime(2018,9,1,18,5,31), SizeInMeters = 0.6},
                new Catch {Fishtype = tuna, DateTime = new DateTime(2018,9,1,18,48,54), SizeInMeters = 1.5},
            };

            fishingTrip3.Catches = new List<Catch>
            {
                new Catch {Fishtype = tuna, DateTime = new DateTime(2019,2,17,15,56,29), SizeInMeters = 1.1},
                new Catch {Fishtype = tuna, DateTime = new DateTime(2019,2,17,17,26,16), SizeInMeters = 1.4},
                new Catch {Fishtype = tuna, DateTime = new DateTime(2019,2,17,17,59,54), SizeInMeters = 1.2},
            };

            fishingTrip4.Catches = new List<Catch>
            {
                new Catch {Fishtype = salmon, DateTime = new DateTime(2019,1,5,17,5,6), SizeInMeters = 0.85},
                new Catch {Fishtype = salmon, DateTime = new DateTime(2019,1,5,17,45,33), SizeInMeters = 0.8},
                new Catch {Fishtype = salmon, DateTime = new DateTime(2019,1,5,18,42,30), SizeInMeters = 0.7},
            };

            fishingTrip5.Catches = new List<Catch>
            {
                new Catch {Fishtype = tuna, DateTime = new DateTime(2018,8,14,4,15,27), SizeInMeters = 1.3},
                new Catch {Fishtype = tuna, DateTime = new DateTime(2018,8,14,5,7,34), SizeInMeters = 1.2},
                new Catch {Fishtype = salmon, DateTime = new DateTime(2018,8,14,7,43,22), SizeInMeters = 0.9},
            };

            _fishingTrips = new ObservableCollection<FishingTrip>
            {
                fishingTrip1,
                fishingTrip2,
                fishingTrip3,
                fishingTrip4,
                fishingTrip5
            };
        }

        private async void TripsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            FishingTrip fishingTrip = e.SelectedItem as FishingTrip;
            await Navigation.PushAsync(new FishingTripDetailsPage(fishingTrip));
            FishingTripsListView.SelectedItem = null;
        }

        private async void AddTrip_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishingTripsFormPage());
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            Console.Write(fishingTrip);
            await Navigation.PushAsync(new FishingTripsFormPage(fishingTrip));
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            FishingTrip fishingTrip = (sender as MenuItem)?.CommandParameter as FishingTrip;
            _fishingTrips.Remove(fishingTrip);
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            FishingTripsListView.ItemsSource = _fishingTrips;
            FishingTripsListView.EndRefresh();
        }
    }
}