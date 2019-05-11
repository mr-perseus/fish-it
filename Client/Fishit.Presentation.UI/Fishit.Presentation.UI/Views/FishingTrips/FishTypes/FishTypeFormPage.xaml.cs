using System;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Fishit.Presentation.UI.Views.FishingTrips.Catches;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.FishTypes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FishTypeFormPage : IPageBase
    {
        private FishType _fishType;
        private bool _isEdit = true;

        public FishTypeFormPage(object caller) : this(caller, new FishType())
        {
        }

        public FishTypeFormPage(object caller, string name) : this(caller, new FishType {Name = name})
        {
        }

        public FishTypeFormPage(object caller, FishType fishType)
        {
            SetCaller(caller);
            SetBindingContext(fishType);
            InitializeComponent();
        }

        public FishTypeListPage CallerFishTypeListPage { get; set; }
        public CatchFormPage CallerCatchFormPage { get; set; }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private void SetCaller(object caller)
        {
            if (caller.GetType() == typeof(FishTypeListPage))
            {
                CallerFishTypeListPage = (FishTypeListPage) caller;
            }
            else if (caller.GetType() == typeof(CatchFormPage))
            {
                CallerCatchFormPage = (CatchFormPage) caller;
            }
        }

        private void SetBindingContext(FishType fishType)
        {
            _fishType = fishType;
            BindingContext = _fishType;
            if (_fishType.Id.Equals("0"))
            {
                _isEdit = false;
            }
        }

        private async Task SaveFishType()
        {
            FishingTripManager manager = new FishingTripManager();
            Response<FishType> response;
            if (_isEdit)
            {
                response = await manager.UpdateFishType(_fishType);
            }
            else
            {
                response = await manager.CreateFishType(_fishType);
            }

            InformUserHelper<FishType> informer =
                new InformUserHelper<FishType>(response, this);
            informer.InformUserOfResponse();

            if (CallerCatchFormPage != null)
            {
                await CallerCatchFormPage.SetFishTypes();
            }

            if (CallerFishTypeListPage != null)
            {
                await CallerFishTypeListPage.ReloadFishTypes();
            }
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