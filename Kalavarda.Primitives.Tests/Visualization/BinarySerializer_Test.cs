using System;
using System.IO;
using System.Linq;
using Kalavarda.Primitives.Visualization;
using NUnit.Framework;

namespace Kalavarda.Primitives.Tests.Visualization
{
    public class BinarySerializer_Test
    {
        [Test]
        public void Serializer_Test()
        {
            var state1 = new State
            {
                Looping = true,
                Name = "MyName 1",
                Sound = new StateSound
                {
                    RawData = CreateRawData()
                }
            };

            var state2 = new State
            {
                Looping = false,
                Name = "MyName 2",
                Views = new []
                {
                    new View
                    {
                        Angle = 321,
                        DurationSec = 1.23f,
                        Frames = new []
                        {
                            new Frame { RawData = CreateRawData() }, 
                        }
                    },
                }
            };

            var obj = new VisualObject
            {
                Id = Guid.NewGuid().ToString(),
                CurrentAngle = 123,
                States = new [] { state1, state2 }
            };
            obj.CurrentState = state1;

            var serializer = new BinarySerializer();
            using var stream1 = new MemoryStream();
            serializer.Serialize(obj, stream1);
            var data = stream1.ToArray();
            
            using var stream2 = new MemoryStream(data);
            var result = serializer.Deserialize<VisualObject>(stream2);

            Assert.AreEqual(obj.Id, result.Id);
            Assert.AreEqual(default(int), result.CurrentAngle);
            Assert.IsNull(result.CurrentState);
            Assert.AreEqual(obj.States.Length, result.States.Length);

            foreach (var state in obj.States)
            {
                var st = result.States.First(s => s.Name == state.Name);
                Assert.AreEqual(state.Looping, st.Looping);
                
                Assert.AreEqual(state.Views.Length, st.Views.Length);
                for (var i = 0; i < state.Views.Length; i++)
                {
                    var view = state.Views[i];
                    var v = st.Views[i];
                    Assert.AreEqual(v.Angle, view.Angle);
                    Assert.AreEqual(v.Duration.TotalSeconds, view.Duration.TotalSeconds, 0.1f);
                    Assert.AreEqual(v.Frames.Length, view.Frames.Length);
                }

                if (state.Sound != null)
                {
                    Assert.AreEqual(state.Sound.RawData.Length, st.Sound.RawData.Length);
                }
                else
                    Assert.IsNull(st.Sound);
            }
        }

        private static byte[] CreateRawData()
        {
            return new byte[] { 1, 2, 3, 4, 5, 6, 7 };
        }
    }
}
