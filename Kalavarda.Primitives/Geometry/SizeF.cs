using System;
using System.Diagnostics;

namespace Kalavarda.Primitives.Geometry
{
    [DebuggerDisplay("{Width}; {Height}")]
    public class SizeF
    {
        private float _width;
        private float _height;

        public float Width
        {
            get => _width;
            set
            {
                if (MathF.Abs(value - _width) < PointF.MinDiff)
                    return;

                _width = value;
                Changed?.Invoke(this);
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                if (MathF.Abs(value - _height) < PointF.MinDiff)
                    return;

                _height = value;
                Changed?.Invoke(this);
            }
        }

        public SizeF()
        {
        }

        public SizeF(float width, float height): this()
        {
            Set(width, height);
        }

        public SizeF DeepClone()
        {
            return new SizeF(Width, Height);
        }

        public void Set(float width, float height)
        {
            var dW = MathF.Abs(Width - width);
            var dH = MathF.Abs(Height - height);
            if (dW > PointF.MinDiff || dH > PointF.MinDiff)
            {
                Width = width;
                Height = height;
                Changed?.Invoke(this);
            }
        }

        public event Action<SizeF> Changed;
    }
}
