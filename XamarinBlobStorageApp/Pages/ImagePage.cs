using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace XamarinBlobStorageApp
{
    public class ImagePage : ContentPage
    {
        readonly List<PhotoModel> _photoList = new List<PhotoModel>();

        readonly Label _title;
        readonly Image _image;
        readonly ActivityIndicator _activityIndicator;

        public ImagePage()
        {
            Title = "Image Page";

            Content = new StackLayout
            {
                Spacing = 15,
                Children =
                {
                    new Image().Assign(out _image),
                    new Label { TextColor = Color.FromHex("1B2A38") }.TextCenter().Assign(out _title),
                    new ActivityIndicator().Assign(out _activityIndicator)
                }
            }.Center();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            _activityIndicator.IsRunning = true;

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            await foreach (var photo in PhotosBlobStorageService.GetPhotos(cancellationTokenSource.Token).ConfigureAwait(false))
            {
                _photoList.Add(photo);

                if (_photoList.Count is 1)
                {
                    var firstPhoto = _photoList.Single();

                    _title.Text = firstPhoto?.Title ?? "Photo Not Found";
                    _image.Source = ImageSource.FromUri(firstPhoto?.Uri);

                    _activityIndicator.IsRunning = false;
                    _activityIndicator.IsVisible = false;
                }
            }
        }
    }
}
