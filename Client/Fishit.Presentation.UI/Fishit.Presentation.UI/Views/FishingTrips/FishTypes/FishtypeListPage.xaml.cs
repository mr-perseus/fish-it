using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.FishTypes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishTypeListPage : ContentPage, IPageBase
    {
        private ObservableCollection<FishType> _fishtypes;

        public FishTypeListPage()
        {
            InitializeComponent();
        }

        public static Task<FishTypeListPage> CreateAsync()
        {
            FishTypeListPage instance = new FishTypeListPage();
            return instance.InitializeAsync();
        }

        private async Task<FishTypeListPage> InitializeAsync()
        {
            await ReloadFishTypes();
            return this;
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        public async Task ReloadFishTypes()
        {
            Response<List<FishType>> response = await new FishingTripManager().GetAllFishTypes();
            _fishtypes = new ObservableCollection<FishType>(response.Content);

            InformUserHelper<List<FishType>> informer =
                new InformUserHelper<List<FishType>>(response, this);
            informer.InformUserOfResponse();

            FishtypeListView.ItemsSource = _fishtypes;
        }

        private async void FishtypeListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }

            FishType fishType = e.SelectedItem as FishType;
            await Navigation.PushAsync(new FishTypeFormPage(this, fishType));
            FishtypeListView.SelectedItem = null;
        }

        private async void Handle_Refreshing(object sender, EventArgs e)
        {
            await ReloadFishTypes();
            FishtypeListView.EndRefresh();
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            FishType fishType = (sender as MenuItem)?.CommandParameter as FishType;
            await Navigation.PushAsync(new FishTypeFormPage(this, fishType));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            FishType fishType = (sender as MenuItem)?.CommandParameter as FishType;
            FishingTripManager manager = new FishingTripManager();
            Response<FishType> response = await manager.DeleteFishType(fishType);

            InformUserHelper<FishType> informer =
                new InformUserHelper<FishType>(response, this);
            informer.InformUserOfResponse("Fish type has been deleted successfully.");

            await ReloadFishTypes();
        }

        private async void BtnAddFishType_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishTypeFormPage(this));
        }
    }
}