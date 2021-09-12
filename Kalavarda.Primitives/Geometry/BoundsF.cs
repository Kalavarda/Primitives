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

        public abstract bool DoesIntersect(float x, float y);

        public bool DoesIntersect(PointF p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));
            return DoesIntersect(p.X, p.Y);
        }

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

        public override bool DoesIntersect(float x, float y)
        {
            throw new NotImplementedException();
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
        public override bool DoesIntersect(float x, float y)
        {
            var x1 = Position.X - Width / 2;
            var x2 = Position.X + Width / 2;
            var y1 = Position.Y - Height / 2;
            var y2 = Position.Y + Height / 2;
            var minX = MathF.Min(x1, x2);
            var maxX = MathF.Max(x1, x2);
            var minY = MathF.Min(y1, y2);
            var maxY = MathF.Max(y1, y2);

            if (x < minX || x > maxX)
                return false;

            if (y < minY || y > maxY)
                return false;

            return true;
        }

        public override bool DoesIntersect(BoundsF b)
        {
            throw new NotImplementedException();
        }

        public override BoundsF DeepClone()
        {
            throw new NotImplementedException();
        }

        public RectBounds(PointF center, SizeF size) : base(center, size)
        {
        }

        public RectBounds(PointF center) : base(center, new SizeF())
        {
        }
    }
}
