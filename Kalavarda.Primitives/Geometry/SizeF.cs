using System.Diagnostics;

namespace Kalavarda.Primitives.Geometry
{
    [DebuggerDisplay("{Width}; {Height}")]
    public class SizeF
    {
        public float Width { get; set; }

        public float Height { get; set; }
    }
}
