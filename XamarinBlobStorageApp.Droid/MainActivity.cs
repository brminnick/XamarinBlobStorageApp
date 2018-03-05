using Android.App;
using Android.Content.PM;
using Android.OS;

using Plugin.Permissions;

namespace XamarinBlobStorageApp.Droid
{
    [Activity(Label = "XamarinBlobStorageApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults) =>
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Droid.CachedImageRenderer.Init(true);
            EntryCustomReturn.Forms.Plugin.Android.CustomReturnEntryRenderer.Init();

            LoadApplication(new App());
        }
    }
}
