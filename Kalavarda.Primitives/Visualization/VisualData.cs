using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;

namespace Kalavarda.Primitives.Visualization
{
    public class VisualData
    {
        private static readonly State[] NoStates = new State[0];
        private State _state;
        private int _angle;

        /// <summary>
        /// Current state
        /// </summary>
        [JsonIgnore]
        public State State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;

                if (value != null)
                {
                    if (!States.Contains(value))
                        throw new ArgumentException("Unknown state");
                }

                _state = value;
                StateChanged?.Invoke(this);
            }
        }

        public State[] States { get; set; } = NoStates;

        public event Action<VisualData> StateChanged;

        /// <summary>
        /// Current angle (degrees)
        /// </summary>
        [JsonIgnore]
        public int Angle
        {
            get => _angle;
            set
            {
                var a = value;
                while (a > 180)
                    a -= 360;
                while (a <= -180)
                    a += 360;

                if (a == _angle)
                    return;

                _angle = a;
                AngleChanged?.Invoke(this);
            }
        }

        public event Action<VisualData> AngleChanged;
    }

    [DebuggerDisplay("{Name}")]
    public class State
    {
        private static readonly View[] NoViews = new View[0];

        public string Name { get; set; }

        public View[] Views { get; set; } = NoViews;

        /// <summary>
        /// Возвращает вид, который ближе всего к запрошенному углу поворота
        /// </summary>
        public View GetView(int angle)
        {
            return Views.FirstOrDefault(); // TODO: implement this
        }
    }
}
