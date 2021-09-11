using Kalavarda.Primitives.Geometry;

namespace Kalavarda.Primitives.Abstract
{
    public interface IPhysicalObject
    {
        BoundsF Bounds { get; }

        float Speed { get; }

        /// <summary>
        /// Направление (угол поворота)
        /// </summary>
        AngleF Direction { get; }
    }
}
