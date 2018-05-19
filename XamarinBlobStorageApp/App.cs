using System.Linq;

using Xamarin.Forms;

namespace XamarinBlobStorageApp
{
    public class App : Application
    {
		public App() => MainPage = new NavigationPage(new ImagePage());
    }

    public class ImagePage : ContentPage
    {
        readonly Label _title = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
			TextColor = Color.FromHex("1B2A38")
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

            _title.Text = firstPhoto?.Title ?? "Photo Not Found";
            _image.Source = ImageSource.FromUri(firstPhoto?.Uri);

            _activityIndicator.IsRunning = false;
            _activityIndicator.IsVisible = false;
        }
    }
}
