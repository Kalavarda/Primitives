using Kalavarda.Primitives.Geometry;

namespace Kalavarda.Primitives.Abstract
{
    public interface IHasBounds
    {
        BoundsF Bounds { get; }
    }
}
