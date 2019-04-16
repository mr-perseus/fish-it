using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fishit.Dal.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Catches
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CatchesListPage : ContentPage
    {
        private ObservableCollection<Catch> _catches;
        private FishingTrip FishingTrip;
		public CatchesListPage(FishingTrip fishingTrip)
        {
            FishingTrip = fishingTrip;
            _catches = new ObservableCollection<Catch>(fishingTrip.Catches);
			InitializeComponent ();
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

        private void Delete_Clicked(object sender, EventArgs e)
        {
            Catch _catch= (sender as MenuItem)?.CommandParameter as Catch;
            _catches.Remove(_catch);
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {
            CatchesListView.ItemsSource = _catches;
            CatchesListView.EndRefresh();
        }
    }
}