using System;

namespace XamarinBlobStorageApp
{
    public class PhotoModel
    {
        public PhotoModel(Uri uri, string title) =>
            (Uri, Title) = (uri, title);

        public Uri Uri { get; }
        public string Title { get; }
    }
}
