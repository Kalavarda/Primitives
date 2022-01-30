using Kalavarda.Primitives.Geometry;

namespace Kalavarda.Primitives.Visualization
{
    public class Layer
    {
        public bool IsHidden { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public IReadonlyVisualObject VisualObjects { get; set; }

        public ITwoDimensionalObject[] Objects { get; set; }
    }

    public interface ITwoDimensionalObject
    {
        public PointF Center { get; }
    }

    public class Texture: ITwoDimensionalObject
    {
        /// <inheritdoc/>
        public PointF Center { get; set; } = new PointF();
    }
}
