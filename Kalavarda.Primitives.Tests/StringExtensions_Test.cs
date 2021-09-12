using Kalavarda.Primitives.Utils;
using NUnit.Framework;

namespace Kalavarda.Primitives.Tests
{
    public class StringExtensions_Test
    {
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        [TestCase(0.5f, "0.5")]
        [TestCase(-0.5f, "-0.5")]
        [TestCase(12.345f, "12.3")]
        [TestCase(123.45f, "123")]
        public void ToStr_Float_Test(float value, string expected)
        {
            var actual = value.ToStr();
            Assert.AreEqual(expected, actual);
        }
    }
}
