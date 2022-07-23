using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Kalavarda.Primitives.WPF.Map;

namespace Kalavarda.Primitives.WPF.Controls
{
    public partial class MapTextureControl
    {
        private MapTexture _mapTexture;

        public MapTexture MapTexture
        {
            get => _mapTexture;
            set
            {
                if (_mapTexture == value)
                    return;

                _mapTexture = value;

                if (_mapTexture != null)
                {
                    Width = _mapTexture.Size.Width;
                    Height = _mapTexture.Size.Height;

                    _imageBrush.ImageSource = _mapTexture.ImageSource;
                    if (_mapTexture.ImageSource is BitmapImage bitmapImage)
                        _imageBrush.Viewport = new Rect(0, 0, bitmapImage.Width, bitmapImage.Height);
                    _scaleTransform.ScaleX = _scaleTransform.ScaleY = _mapTexture.Scale;
                }
                else
                    Background = Brushes.Transparent;
            }
        }

        public MapTextureControl()
        {
            InitializeComponent();
        }
    }
}
