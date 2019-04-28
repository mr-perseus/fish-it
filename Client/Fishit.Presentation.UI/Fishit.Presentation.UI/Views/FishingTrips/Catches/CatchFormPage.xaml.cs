using System;
using System.Threading.Tasks;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatchFormPage : ContentPage, IPageBase
    {
        private FishingTrip FishingTrip { get; set; }

        public CatchFormPage(FishingTrip fishingTrip) : this(fishingTrip, new Catch())
        {
        }

        public CatchFormPage(FishingTrip fishingTrip, Catch _catch)
        {
            SetBindingContext(fishingTrip, _catch);
            InitializeComponent();
        }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Catch Catch { get; set; }
        public bool IsEdit { get; set; }

        private async Task SaveCatch()
        {
            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response;
            if (IsEdit)
                response = await manager.UpdateCatch(FishingTrip, Catch);
            else
                response = await manager.AddCatch(FishingTrip, Catch);

            InformUserHelper<FishingTrip> informer = new InformUserHelper<FishingTrip>(response, this, "Catch has been saved successfully!");
            informer.InformUserOfResponse();
        }

        private async void BtnCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveCatch_OnClicked(object sender, EventArgs e)
        {
            await SaveCatch();
            await Navigation.PopAsync();
            await Navigation.PopAsync();
        }

        private void SetBindingContext(FishingTrip fishingTrip, Catch _catch)
        {
            FishingTrip = fishingTrip;
            Catch = _catch;
            BindingContext = _catch;
            if (!_catch.Id.Equals("0"))
                IsEdit = true;
            else
                Catch.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, DateTime.Now.Minute, 0);

            Date = Catch.DateTime.Date;
            Time = Catch.DateTime.TimeOfDay;
        }

        public void Refresh_DateTime(object sender, EventArgs e)
        {
            FishingTrip.DateTime = new DateTime(
                Date.Year,
                Date.Month,
                Date.Day,
                Time.Hours,
                Time.Minutes,
                0);
        }
        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }
    }
}