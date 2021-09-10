using Kalavarda.Primitives.Geometry;
using NUnit.Framework;

namespace Kalavarda.Primitives.Tests
{
    public class RoundBounds_Tests
    {
        [TestCase(4, 2, true)]
        [TestCase(4.24f, 2, true)]
        [TestCase(3.76f, 2, true)]
        [TestCase(4, 2.24f, true)]
        [TestCase(4, 1.76f, true)]
        [TestCase(4.25f, 2.25f, false)]
        [TestCase(3.75f, 1.75f, false)]
        public void IntersectPoint_Test(float x, float y, bool intersect)
        {
            var bounds = new RoundBounds(new PointF(4, 2), 0.25f);
            Assert.AreEqual(intersect, bounds.DoesIntersect(new PointF(x, y)));
        }

        [TestCase(4, 2, 1f, true)]
        [TestCase(3, 2, 0.5f, true)]
        [TestCase(2, 2, 1f, false)]
        [TestCase(3, 1, 0.5f, true)]
        [TestCase(3, 1, 0.1f, false)]
        public void IntersectRound_Test(float x, float y, float r, bool intersect)
        {
            var bounds = new RoundBounds(new PointF(4, 2), 1f);
            Assert.AreEqual(intersect, bounds.DoesIntersect(new RoundBounds(new PointF(x, y), r)));
        }
    }
}
