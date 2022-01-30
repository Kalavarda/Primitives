using Kalavarda.Primitives.Geometry;
using NUnit.Framework;

namespace Kalavarda.Primitives.Tests
{
    public class RectBounds_Tests
    {
        [TestCase(0, 0, true)]
        [TestCase(10, 10, true)]
        [TestCase(-10, 10, true)]
        [TestCase(0, 10, true)]
        [TestCase(0, 10.1f, false)]
        [TestCase(10.1f, 10.1f, false)]
        [TestCase(-10.1f, 0, false)]
        public void DoesIntersect_Test(float x, float y, bool intersect)
        {
            var bounds = new RectBounds(new PointF(0, 0));
            bounds.Size.Width = 20;
            bounds.Size.Height = 20;

            Assert.AreEqual(intersect, bounds.DoesIntersect(new PointF(x, y)));
        }

        [TestCase(4, 2, 3, 1, true)]
        [TestCase(40, 20, 3, 1, false)]
        [TestCase(5.5f, 2.5f, 1, 1, true)]
        [TestCase(6, 3, 1, 1, false)]
        public void IntersectRect_Test(float x, float y, float w, float h, bool intersect)
        {
            var bounds1 = new RectBounds(new PointF(4, 2), new SizeF(3, 1));
            var bounds2 = new RectBounds(new PointF(x, y), new SizeF(w, h));
            Assert.AreEqual(intersect, bounds1.DoesIntersect(bounds2));
        }
    }
}
