using Xamarin.Forms;
using System.Linq;

namespace XamarinBlobStorageApp
{
    public class App : Application
    {
        //public App() => MainPage = new BaseNavigationPage(new PhotoListPage());
        public App()
        {
            MainPage = new NavigationPage(new ImagePage());
        }
    }

    public class ImagePage : ContentPage
    {
        readonly Label _title = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            TextColor = ColorConstants.DetailColor
        };
        readonly Image _image = new Image();
        readonly ActivityIndicator _activityIndicator = new ActivityIndicator();

        public ImagePage()
        {
            Content = new StackLayout
            {
                Spacing = 15,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    _title,
                    _image,
                    _activityIndicator
                }
            };

            Title = "Image Page";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _activityIndicator.IsRunning = true;

            var photoList = await PhotosBlobStorageService.GetPhotos();

            var firstPhoto = photoList?.FirstOrDefault();

            _title.Text = firstPhoto?.Title;
            _image.Source = ImageSource.FromUri(firstPhoto?.Uri);

            _activityIndicator.IsRunning = false;
            _activityIndicator.IsVisible = false;
        }
    }
}
