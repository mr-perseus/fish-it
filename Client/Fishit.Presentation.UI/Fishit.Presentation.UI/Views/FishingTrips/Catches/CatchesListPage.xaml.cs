using System;
using System.Collections.ObjectModel;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatchesListPage : ContentPage, IPageBase
    {
        private readonly ObservableCollection<Catch> _catches;
        public FishingTrip FishingTrip { get; set; }

        public CatchesListPage(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            _catches = new ObservableCollection<Catch>(fishingTrip.Catches);
            InitializeComponent();
            CatchesListView.ItemsSource = _catches;
        }

        private async void CatchesListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            Catch _catch = e.SelectedItem as Catch;
            Console.WriteLine(_catch);
            await Navigation.PushAsync(new CatchFormPage(FishingTrip, _catch));
            CatchesListView.SelectedItem = null;
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            Catch _catch = (sender as MenuItem)?.CommandParameter as Catch;
            Console.Write(_catch);
            await Navigation.PushAsync(new CatchFormPage(FishingTrip, _catch));
        }

        private async void BtnAddCatch_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CatchFormPage(FishingTrip));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            Catch _catch = (sender as MenuItem)?.CommandParameter as Catch;
            _catches.Remove(_catch);
            
            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response= await manager.DeleteCatch(FishingTrip, _catch);

            InformUserHelper<FishingTrip> informer = new InformUserHelper<FishingTrip>(response, this, "Catch has been saved successfully!");
            informer.InformUserOfResponse();
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            CatchesListView.ItemsSource = _catches;
            CatchesListView.EndRefresh();
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }
    }
}