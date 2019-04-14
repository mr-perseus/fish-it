using System;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatchForm : ContentPage
    {
       
        public CatchForm()
        {
            InitializeComponent();
           
        }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        private FishingTrip FishingTrip { get; }
        private async Task SaveCatch()
        {
            await DisplayAlert("Catch Entry", "Saved Successfully", "Ok");
        }

        private async void CancelCatchForm_OnClicked(object sender, EventArgs e)
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

