using System.Windows.Media;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;

namespace Kalavarda.Primitives.WPF.Map
{
    public class MapTexture: IMapObject
    {
        private readonly RectBounds _bounds;

        public PointF Position { get; } = new();

        public BoundsF Bounds => _bounds;

        public SizeF Size => _bounds.Size;

        public ImageSource ImageSource { get; set; }

        public double Scale { get; set; }

        public MapTexture()
        {
            _bounds = new RectBounds(Position, new SizeF(5, 5));
        }
    }
}
