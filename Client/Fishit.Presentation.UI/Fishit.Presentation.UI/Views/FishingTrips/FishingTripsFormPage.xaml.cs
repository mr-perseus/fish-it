using System;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripsFormPage : ContentPage
    {
        public FishingTripsFormPage()
        {
            InitializeComponent();
            FishingTrip fishingTrip = new FishingTrip {Name = "new name...", Location = "new location..."};
            BindingContext = fishingTrip;
        }

        public FishingTripsFormPage(FishingTrip fishingTrip)
        {
            BindingContext = fishingTrip;
        }

        private async Task CreateFishingTrip()
        {
            await DisplayAlert("Fishing Trip", "Created Successfully", "Oki");
        }

        private async void CancelForm_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void CreateFishingTrip_OnClicked(object sender, EventArgs e)
        {
            await CreateFishingTrip();
            await Navigation.PopAsync();
        }
    }
}