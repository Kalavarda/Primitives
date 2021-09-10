using NUnit.Framework;

namespace Kalavarda.Primitives.Tests
{
    public class RangeF_Tests
    {
        [TestCase(0, 100)]
        [TestCase(-50, 150)]
        [TestCase(-0.5f, 100500)]
        public void Value_Test(float min, float max)
        {
            var eventCount = 0;

            var range = new RangeF(min, max);
            range.ValueChanged += obj =>
            {
                eventCount++;
            };

            range.Value = (min + max) / 2;
            Assert.AreEqual((min + max) / 2, range.Value);
            Assert.AreEqual(1, eventCount);

            range.Value = (min + max) / 2;
            Assert.AreEqual((min + max) / 2, range.Value);
            Assert.AreEqual(1, eventCount);

            range.Value = max + 1;
            Assert.AreEqual(max, range.Value);
            Assert.AreEqual(2, eventCount);

            range.Value = max + 1;
            Assert.AreEqual(max, range.Value);
            Assert.AreEqual(2, eventCount);

            range.Value = min - 1;
            Assert.AreEqual(min, range.Value);
            Assert.AreEqual(3, eventCount);

            range.Value = min - 1;
            Assert.AreEqual(min, range.Value);
            Assert.AreEqual(3, eventCount);
        }

        [TestCase(0, 100)]
        [TestCase(-50, 150)]
        [TestCase(-0.5f, 100500)]
        public void Max_Test(float min, float max)
        {
            var eventCount = 0;

            var range = new RangeF(min, max) { Value = max };
            range.MaxChanged += obj =>
            {
                eventCount++;
            };

            range.Max++;
            Assert.AreEqual(max + 1, range.Max);
            Assert.AreEqual(eventCount, 1);

            range.Max = max + 1;
            Assert.AreEqual(eventCount, 1);

            range.Value = range.Max;
            Assert.AreEqual(max + 1, range.Value);

            range.Max = max;
            Assert.AreEqual(eventCount, 2);
            Assert.AreEqual(max, range.Value);
        }

        [TestCase(0, 50, 100, 0.5f)]
        [TestCase(-4.5f, -4.5f, 87, 0.0f)]
        [TestCase(-4.5f, 87, 87, 1.0f)]
        [TestCase(-10, 5, 20, 0.5f)]
        public void ValueN(float min, float value, float max, float expected)
        {
            var range = new RangeF(min, max) { Value = value };
            Assert.AreEqual(expected, range.ValueN);
        }
    }
}
