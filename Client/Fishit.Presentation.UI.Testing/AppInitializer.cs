using Xamarin.UITest;

namespace Fishit.Presentation.UI.Testing
{
    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android
                    .ApkFile("../../../Fishit.Presentation.UI/Fishit.Presentation.UI.Android/bin/Release/ch.fishit.apk")
                    .StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}