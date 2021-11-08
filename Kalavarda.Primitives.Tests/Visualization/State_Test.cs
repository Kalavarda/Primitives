using Kalavarda.Primitives.Visualization;
using NUnit.Framework;

namespace Kalavarda.Primitives.Tests.Visualization
{
    public class State_Test
    {
        [TestCase(0, 0)]
        [TestCase(40, 0)]
        [TestCase(50, 90)]
        [TestCase(170, 180)]
        [TestCase(190, 180)]
        [TestCase(-40, 0)]
        [TestCase(-50, -90)]
        [TestCase(-100, -90)]
        [TestCase(-170, 180)]
        public void GetView_Test(int a, int expectedAngle)
        {
            var state = new State
            {
                Views = new []
                {
                    new View { Angle = -90 },
                    new View { Angle = 0 }, 
                    new View { Angle = 90 }, 
                    new View { Angle = 180 }
                }
            };
            var view = state.GetView(a);
            Assert.AreEqual(expectedAngle, view.Angle);
        }
    }
}
