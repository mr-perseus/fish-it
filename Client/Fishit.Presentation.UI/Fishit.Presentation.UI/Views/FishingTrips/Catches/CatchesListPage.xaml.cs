using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Fishit.Presentation.UI.Views.FishingTrips.FishTypes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatchesListPage : ContentPage, IPageBase
    {
        private ObservableCollection<Catch> _catches;

        public CatchesListPage(FishingTrip fishingTrip)
        {
            SetBindingContext(fishingTrip);
            InitializeComponent();
            CatchesListView.ItemsSource = _catches;
        }

        public FishingTrip FishingTrip { get; set; }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private async void CatchesListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            Catch _catch = e.SelectedItem as Catch;
            Console.WriteLine(_catch);
            CatchFormPage catchFormPage = await CatchFormPage.CreateAsync(this, FishingTrip, _catch);
            await Navigation.PushAsync(catchFormPage);
            CatchesListView.SelectedItem = null;
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            Catch _catch = (sender as MenuItem)?.CommandParameter as Catch;
            Console.Write(_catch);
            CatchFormPage catchFormPage = await CatchFormPage.CreateAsync(this, FishingTrip, _catch);
            await Navigation.PushAsync(catchFormPage);
        }

        private async void BtnAddCatch_OnClicked(object sender, EventArgs e)
        {
            CatchFormPage catchFormPage = await CatchFormPage.CreateAsync(this, FishingTrip);
            await Navigation.PushAsync(catchFormPage);
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            Catch _catch = (sender as MenuItem)?.CommandParameter as Catch;

            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response = await manager.DeleteCatch(FishingTrip, _catch);

            InformUserHelper<FishingTrip> informer =
                new InformUserHelper<FishingTrip>(response, this);
            informer.InformUserOfResponse("Catch has been deleted successfully!");

            await ReloadFishingTrip();
        }

        private async void Handle_Refreshing(object sender, EventArgs e)
        {
            await ReloadFishingTrip();
            RefreshList(FishingTrip);
            CatchesListView.EndRefresh();
        }

        private void SetBindingContext(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            _catches = new ObservableCollection<Catch>(FishingTrip.Catches);
        }

        public void RefreshList(FishingTrip fishingTrip)
        {
            SetBindingContext(fishingTrip);
            CatchesListView.ItemsSource = _catches;
        }

        private async Task ReloadFishingTrip()
        {
            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response = await manager.GetFishingTripById(FishingTrip.Id);

            InformUserHelper<FishingTrip> informer = 
                new InformUserHelper<FishingTrip>(response, this);

            informer.InformUserOfResponse();

            RefreshList(response.Content);
        }

        private async void BtnManageFishTypes_OnClicked(object sender, EventArgs e)
        {
            FishTypeListPage fishTypeListPage = await FishTypeListPage.CreateAsync();
            await Navigation.PushAsync(fishTypeListPage);
        }
    }
}