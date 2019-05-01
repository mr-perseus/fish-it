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
            SetFishTypes();
            InitializeComponent();
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private async void SetFishTypes()
        {
            Response<List<FishType>> response = await new FishingTripManager().GetAllFishTypes();
            _fishtypes = new ObservableCollection<FishType>(response.Content);

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
            await Navigation.PushAsync(new FishTypeFormPage(fishType));
            FishtypeListView.SelectedItem = null;
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            SetFishTypes();
            FishtypeListView.EndRefresh();
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            FishType fishType = (sender as MenuItem)?.CommandParameter as FishType;
            await Navigation.PushAsync(new FishTypeFormPage(fishType));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            FishType fishType = (sender as MenuItem)?.CommandParameter as FishType;
            FishingTripManager manager = new FishingTripManager();
            Response<FishType> response = await manager.DeleteFishType(fishType);

            InformUserHelper<FishType> informer =
                new InformUserHelper<FishType>(response, this, "Fish type has been deleted successfully.");
            informer.InformUserOfResponse();
        }

        private async void BtnAddFishType_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FishTypeFormPage());
        }
    }
}