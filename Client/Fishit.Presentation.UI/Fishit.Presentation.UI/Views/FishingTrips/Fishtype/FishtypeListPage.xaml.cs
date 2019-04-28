using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views.FishingTrips.Fishtype
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FishtypeListPage : ContentPage
	{
		public FishtypeListPage ()
		{
			InitializeComponent ();
		}

        private void FishtypeListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        private void Handle_Refreshing(object sender, EventArgs e)
        {

        }

        private void Edit_Clicked(object sender, EventArgs e)
        {

        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}