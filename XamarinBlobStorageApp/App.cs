using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace XamarinBlobStorageApp
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            Device.SetFlags(new[] { "Markup_Experimental" });

            var navigationPage = new Xamarin.Forms.NavigationPage(new ImagePage());
            navigationPage.On<iOS>().SetPrefersLargeTitles(true);

            MainPage = navigationPage;
        }
    }
}
