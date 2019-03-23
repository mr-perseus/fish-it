﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Fishit.Presentation.UI.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Fishit.Presentation.UI
{
	public class App : Application
	{
        public App()
        {
            MainPage = new SplashScreen();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}