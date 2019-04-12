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
     //   public CatchForm() : this(new Catch()) { }
        public CatchForm()
        {
            InitializeComponent();
           
        }

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
    }
}

