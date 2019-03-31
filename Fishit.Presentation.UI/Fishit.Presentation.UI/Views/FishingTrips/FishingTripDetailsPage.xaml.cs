using System;
using Fishit.Presentation.UI.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishingTripDetailsPage : ContentPage
    {
        public FishingTripDetailsPage()
        {
            InitializeComponent();
        }
        public FishingTripDetailsPage(FishingTrip fishingTrip)
        {
            BindingContext = fishingTrip;

            InitializeComponent();
        }
    }
}