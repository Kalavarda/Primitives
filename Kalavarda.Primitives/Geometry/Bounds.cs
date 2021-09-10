using System;

namespace Kalavarda.Primitives.Geometry
{
    public abstract class Bounds
    {
        public PointF Center { get; }

        public abstract bool DoesIntersect(PointF p);

        public abstract bool DoesIntersect(Bounds b);

        public abstract float Width { get; }

        public abstract float Height { get; }

        protected Bounds(PointF center)
        {
            Center = center ?? throw new ArgumentNullException(nameof(center));
        }

        public abstract Bounds DeepClone();
    }

    public class RoundBounds : Bounds
    {
        public float Radius { get; }

        public RoundBounds(PointF center, float radius) : base(center)
        {
            Radius = radius;
        }

        public override bool DoesIntersect(PointF p)
        {
            return Center.DistanceTo(p) <= Radius;
        }

        public override bool DoesIntersect(Bounds b)
        {
            if (b == null) throw new ArgumentNullException(nameof(b));

            if (b is RoundBounds round)
                return Center.DistanceTo(round.Center) < Radius + round.Radius;

            throw new NotImplementedException();
        }

        public override float Width => 2 * Radius;

        public override float Height => 2 * Radius;

        public override Bounds DeepClone()
        {
            return new RoundBounds(Center.DeepClone(), Radius);
        }
    }

    public class RectBounds : Bounds
    {
        public override bool DoesIntersect(PointF p)
        {
            throw new NotImplementedException();
        }

        public override bool DoesIntersect(Bounds b)
        {
            throw new NotImplementedException();
        }

        public override float Width { get; } = default;

        public override float Height { get; } = default;

        public override Bounds DeepClone()
        {
            throw new NotImplementedException();
        }

        public RectBounds(PointF center) : base(center)
        {
        }
    }
}
