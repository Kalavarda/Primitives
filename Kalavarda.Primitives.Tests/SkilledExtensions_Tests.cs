using System;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Moq;
using NUnit.Framework;

namespace Kalavarda.Primitives.Tests
{
    public class SkilledExtensions_Tests
    {
        static TestDataItem[] TestData =
        {
            new TestDataItem
            {
                Skills = new ISkill[]
                {
                    new TestSkill(TimeSpan.Zero) { MaxDistance = 100 },
                    new TestSkill(TimeSpan.Zero) { MaxDistance = 10 },
                    new TestSkill(TimeSpan.Zero) { MaxDistance = 1 }
                },
                Result = 100
            },
            new TestDataItem
            {
                Skills = new ISkill[]
                {
                    new TestSkill(TimeSpan.FromSeconds(1.5)) { MaxDistance = 100 },
                    new TestSkill(TimeSpan.Zero) { MaxDistance = 10 },
                    new TestSkill(TimeSpan.FromSeconds(0.5)) { MaxDistance = 1 }
                },
                Result = 10
            },
            new TestDataItem
            {
                Skills = new ISkill[]
                {
                    new TestSkill(TimeSpan.FromSeconds(0.5)) { MaxDistance = 100 },
                    new TestSkill(TimeSpan.FromSeconds(1.5)) { MaxDistance = 10 },
                    new TestSkill(TimeSpan.FromSeconds(2.5)) { MaxDistance = 1 }
                },
                Result = 100
            },
        };

        [TestCaseSource(nameof(TestData))]
        public void GetMaxAttackDistance_Test(TestDataItem item)
        {
            var skilled = new Mock<ISkilled>();
            skilled
                .Setup(s => s.Skills)
                .Returns(item.Skills);
            var distance = skilled.Object.GetMaxSkillDistance();
            Assert.AreEqual(item.Result, distance, 0.01);
        }

        public class TestDataItem
        {
            public ISkill[] Skills { get; set; }

            public float Result { get; set; }
        }

        private class TestSkill : ISkill
        {
            private readonly Mock<ITimeLimiter> _timeLimiter = new Mock<ITimeLimiter>();

            public string Name => string.Empty;

            public float MaxDistance { get; set; }

            public ITimeLimiter TimeLimiter { get; }

            public TestSkill(TimeSpan remain)
            {
                _timeLimiter.Setup(tl => tl.Remain).Returns(remain);
                TimeLimiter = _timeLimiter.Object;
            }

            public IProcess Use()
            {
                throw new NotImplementedException();
            }

            public TimeSpan Cooldown { get; set; }
        }
    }
}
