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
        
        public StateSound Sound { get; set; }

        /// <summary>
        /// Зацикливать анимацию и звук
        /// </summary>
        public bool Looping { get; set; }

        /// <summary>
        /// Возвращает вид, который ближе всего к запрошенному углу поворота
        /// </summary>
        public View GetView(int angle)
        {
            if (Views == null || Views.Length == 0)
                return null;

            View nearestView = null;
            var minAngle = 360;
            foreach (var view in Views)
            {
                var a = GetAngle(angle, view.Angle);
                if (a < minAngle)
                {
                    nearestView = view;
                    minAngle = a;
                }
            }

            return nearestView;
        }

        private static int GetAngle(int angle1, int angle2)
        {
            var a = Math.Abs(angle2 - angle1);
            if (a <= 180)
                return a;

            if (angle1 < angle2)
                angle1 += 360;
            else
                angle2 += 360;
            
            return Math.Abs(angle2 - angle1);
        }
    }

    public class StateSound
    {
        public byte[] RawData { get; set; }
    }
}
