using System;
using System.ComponentModel;

namespace XamarinBlobStorageApp
{
    public record PhotoModel(Uri Uri, string Title);
}

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class IsExternalInit { }
}