using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Kalavarda.Primitives.WPF
{
    public class BitmapImageCache
    {
        private readonly IDictionary<Uri, BitmapImage> _dict = new Dictionary<Uri, BitmapImage>();

        public static BitmapImageCache Instance { get; } = new();

        public BitmapImage Get(Uri uri)
        {
            lock (_dict)
            {
                if (_dict.TryGetValue(uri, out var bitmapImage))
                    return bitmapImage;

                bitmapImage = new BitmapImage(uri);
                _dict.Add(uri, bitmapImage);
                return bitmapImage;
            }
        }

        private BitmapImageCache()
        {

        }
    }
}
