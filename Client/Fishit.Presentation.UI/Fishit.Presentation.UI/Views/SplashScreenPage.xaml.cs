using System;
using Xamarin.Forms.Xaml;

namespace Fishit.Presentation.UI.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreenPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
        }

        private async void Next_OnClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AuthPage());
        }
    }
}