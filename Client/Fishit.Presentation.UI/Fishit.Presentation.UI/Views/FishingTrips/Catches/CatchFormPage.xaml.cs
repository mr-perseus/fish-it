using System;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatchFormPage : ContentPage
    {
        private readonly FishingTrip FishingTrip;

        public CatchFormPage(FishingTrip fishingTrip) : this(fishingTrip, new Catch())
        {
        }

        public CatchFormPage(FishingTrip fishingTrip, Catch _catch)
        {
            FishingTrip = fishingTrip;
            Catch = _catch;
            BindingContext = _catch;
            Date = fishingTrip.DateTime.Date;
            Time = fishingTrip.DateTime.TimeOfDay;
            InitializeComponent();
        }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Catch Catch { get; set; }

        private async Task SaveCatch()
        {
            await DisplayAlert("Catch Entry", "This is not yet implemented", "Ok");
        }

        private async void BtnCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveCatch_OnClicked(object sender, EventArgs e)
        {
            await SaveCatch();
            await Navigation.PopAsync();
        }

        public void Refresh_DateTime(object sender, EventArgs e)
        {
            FishingTrip.DateTime = new DateTime(
                Date.Year,
                Date.Month,
                Date.Day,
                Time.Hours,
                Time.Minutes,
                0);
        }
    }
}