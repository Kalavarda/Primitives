using System.Diagnostics;

namespace Kalavarda.Primitives.Geometry
{
    [DebuggerDisplay("{Width}; {Height}")]
    public class SizeF
    {
        public float Width { get; set; }

        public float Height { get; set; }

        public SizeF()
        {
        }

        public SizeF(float width, float height): this()
        {
            Width = width;
            Height = height;
        }

        public SizeF DeepClone()
        {
            return new SizeF(Width, Height);
        }
    }
}
