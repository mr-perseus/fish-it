using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.FishTypes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishtypeListPage : ContentPage, IPageBase
    {
        private ObservableCollection<FishType> _fishtypes;

        public FishtypeListPage()
        {
            ReloadFishTypes();
            InitializeComponent();
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        public async void ReloadFishTypes()
        {
            Response<List<FishType>> response = await new FishingTripManager().GetAllFishTypes();
            _fishtypes = new ObservableCollection<FishType>(response.Content);

            InformUserHelper<List<FishType>> informer =
                new InformUserHelper<List<FishType>>(response, this);
            informer.InformUserOfResponse();

            if (FishtypeListView != null)
            {
                FishtypeListView.ItemsSource = _fishtypes;
            }
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

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            ReloadFishTypes();
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
        }

        private async void BtnAddFishType_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishTypeFormPage(this));
        }
    }
}