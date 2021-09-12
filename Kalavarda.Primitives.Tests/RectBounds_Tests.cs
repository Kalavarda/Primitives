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
    }
}
