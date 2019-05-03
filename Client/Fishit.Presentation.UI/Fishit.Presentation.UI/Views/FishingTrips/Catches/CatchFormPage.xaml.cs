﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Android.Telecom;
using dotMorten.Xamarin.Forms;
using Fishit.BusinessLayer;
using Fishit.Dal.Entities;
using Fishit.Presentation.UI.Helpers;
using Fishit.Presentation.UI.Views.FishingTrips.FishTypes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatchFormPage : ContentPage, IPageBase
    {
        public CatchFormPage(CatchesListPage caller, FishingTrip fishingTrip, Catch _catch)
        {
            Caller = caller;
            SetBindingContext(fishingTrip, _catch);
            InitializeComponent();
        }

        public static Task<CatchFormPage> CreateAsync(CatchesListPage caller, FishingTrip fishingTrip)
        {
            return CreateAsync(caller, fishingTrip, new Catch());
        }

        public static Task<CatchFormPage> CreateAsync(CatchesListPage caller, FishingTrip fishingTrip, Catch _catch)
        {
            CatchFormPage instance = new CatchFormPage(caller, fishingTrip, _catch);
            return instance.InitializeAsync();
        }

        private async Task<CatchFormPage> InitializeAsync()
        {
            await SetFishTypes();
            return this;
        }

        private FishingTrip FishingTrip { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Catch Catch { get; set; }
        public bool IsEdit { get; set; }
        public ObservableCollection<FishType> FishTypes { get; set; }
        public List<string> FishTypesAsStrings { get; set; }
        public string FishType { get; set; }
        public CatchesListPage Caller { get; set; }

        public void DisplayAlertMessage(string title, string message)
        {
            DisplayAlert(title, message, "Ok");
        }

        private async Task SaveCatch()
        {
            FishingTripManager manager = new FishingTripManager();
            Response<FishingTrip> response;
            if (IsEdit)
            {
                response = await manager.UpdateCatch(FishingTrip, Catch);
            }
            else
            {
                response = await manager.AddCatch(FishingTrip, Catch);
            }

            InformUserHelper<FishingTrip> informer =
                new InformUserHelper<FishingTrip>(response, this);
            informer.InformUserOfResponse("Catch has been saved successfully!");
            Caller.RefreshList(response.Content);
        }

        private async void BtnCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void SaveCatch_OnClicked(object sender, EventArgs e)
        {
            Catch.FishType = GetFishType();
            await SaveCatch();
            await Navigation.PopAsync();
        }

        private FishType GetFishType()
        {
            foreach (FishType fishType in FishTypes)
            {
                if (fishType.Name.Equals(FishType))
                {
                    return fishType;
                }
            }

            return new FishType();
        }

        private void SetBindingContext(FishingTrip fishingTrip, Catch _catch)
        {
            FishingTrip = fishingTrip;
            Catch = _catch;
            BindingContext = _catch;
            if (!_catch.Id.Equals("0"))
            {
                IsEdit = true;
                FishType = Catch.FishType.Name;
            }
            else
            {
                Catch.DateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, DateTime.Now.Minute, 0);
                FishType = "";
            }

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

        private async Task SetFishTypes()
        {
            Response<List<FishType>> response = await new FishingTripManager().GetAllFishTypes();

            InformUserHelper<List<FishType>> informer = 
                new InformUserHelper<List<FishType>>(response, this);
            informer.InformUserOfResponse();

            FishTypes = new ObservableCollection<FishType>(response.Content);

            FishTypesAsStrings = new List<string>();
            SetFishTypesAsStrings();
        }

        private void SetFishTypesAsStrings()
        {
            foreach (FishType fishType in FishTypes)
            {
                FishTypesAsStrings.Add(fishType.Name);
            }
        }

        private void FishTypeAutoComplete_OnTextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<string> filteredList =
                    FishTypesAsStrings.Where(x => x.ToLower().StartsWith(FishTypeAutoComplete.Text.ToLower())).ToList();
                FishTypeAutoComplete.ItemsSource = filteredList;
            }
        }

        private void FishTypeAutoComplete_OnSuggestionChosen(object sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            FishType = (string) e.SelectedItem;
        }

        private async void AddFishType_OnClicked(object sender, EventArgs e)
        {
        }
    }
}