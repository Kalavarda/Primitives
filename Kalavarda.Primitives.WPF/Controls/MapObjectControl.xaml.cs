using System.Windows.Controls;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.WPF.Controls
{
    public partial class MapObjectControl
    {
        private IMapObject _mapObject;

        public IMapObject MapObject
        {
            get => _mapObject;
            set
            {
                if (_mapObject == value)
                    return;

                if (_mapObject != null)
                    _mapObject.Position.Changed -= Position_Changed;
                _image.Source = null;

                _mapObject = value;

                if (_mapObject != null)
                {
                    _mapObject.Position.Changed += Position_Changed;
                    Position_Changed(_mapObject.Position);

                    Width = _mapObject.Bounds.Width;
                    Height = _mapObject.Bounds.Height;

                    if (_mapObject is IHasImage hasImage)
                        _image.Source = BitmapImageCache.Instance.Get(hasImage.ImageUri);
                }
            }
        }

        private void Position_Changed(Geometry.PointF pos)
        {
            this.Do(() =>
            {
                Canvas.SetLeft(this, pos.X);
                Canvas.SetTop(this, pos.Y);
            });
        }

        public MapObjectControl()
        {
            InitializeComponent();

            Unloaded += (_, _) => MapObject = null;
        }
    }
}
