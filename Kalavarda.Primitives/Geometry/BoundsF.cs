using System;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Geometry
{
    public abstract class BoundsF: IHasPosition
    {
        /// <summary>
        /// Center
        /// </summary>
        public PointF Position { get; }

        public abstract bool DoesIntersect(PointF p);

        public abstract bool DoesIntersect(BoundsF b);

        public SizeF Size { get; }

        public float Width => Size.Width;
        
        public float Height => Size.Height;

        protected BoundsF(PointF center, SizeF size)
        {
            Position = center ?? throw new ArgumentNullException(nameof(center));
            Size = size ?? throw new ArgumentNullException(nameof(size));
        }

        public abstract BoundsF DeepClone();
    }

    public class RoundBounds : BoundsF
    {
        public float Radius { get; }

        public RoundBounds(PointF center, float radius) : base(center, new SizeF { Width = 2 * radius, Height = 2 * radius })
        {
            Radius = radius;
        }

        public override bool DoesIntersect(PointF p)
        {
            return Position.DistanceTo(p) <= Radius;
        }

        public override bool DoesIntersect(BoundsF b)
        {
            if (b == null) throw new ArgumentNullException(nameof(b));

            if (b is RoundBounds round)
                return Position.DistanceTo(round.Position) < Radius + round.Radius;

            throw new NotImplementedException();
        }

        public override BoundsF DeepClone()
        {
            return new RoundBounds(Position.DeepClone(), Radius);
        }
    }

    public class RectBounds : BoundsF
    {
        public override bool DoesIntersect(PointF p)
        {
            throw new NotImplementedException();
        }

        public override bool DoesIntersect(BoundsF b)
        {
            throw new NotImplementedException();
        }

        public override BoundsF DeepClone()
        {
            throw new NotImplementedException();
        }

        public RectBounds(PointF center) : base(center, new SizeF())
        {
        }
    }
}
