using System;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.FishTypes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishTypeFormPage : ContentPage, IPageBase
    {
        private FishType _fishType;
        private bool _isEdit;

        public FishTypeFormPage() : this(new FishType())
        {
        }

        public FishTypeFormPage(FishType fishType)
        {
            SetBindingContext(fishType);
            InitializeComponent();
        }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private void SetBindingContext(FishType fishType)
        {
            _fishType = fishType;
            BindingContext = _fishType;
            if (!_fishType.Id.Equals("0")) _isEdit = true;
        }

        private async Task SaveFishType()
        {
            FishingTripManager manager = new FishingTripManager();
            Response<FishType> response;
            if (_isEdit)
                response = await manager.UpdateFishType(_fishType);
            else
                response = await manager.CreateFishType(_fishType);

            InformUserHelper<FishType> informer =
                new InformUserHelper<FishType>(response, this, "FishType has been saved successfully!");
            informer.InformUserOfResponse();
        }

        private async void BtnCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void BtnSaveFishType_OnClicked(object sender, EventArgs e)
        {
            await SaveFishType();
            await Navigation.PopAsync();
        }
    }
}