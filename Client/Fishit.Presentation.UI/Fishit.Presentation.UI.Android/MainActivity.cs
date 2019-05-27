using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Fishit.Presentation.UI.Droid
{
    [Activity(Label = "Fish IT", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            FormsMaps.Init(this, savedInstanceState);
            LoadApplication(new App());

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}