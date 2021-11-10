using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;

namespace Kalavarda.Primitives.Visualization
{
    public class VisualObject
    {
        private static readonly State[] NoStates = new State[0];
        private State _currentState;
        private int _currentAngle;

        public string Id { get; set; }

        /// <summary>
        /// Current state
        /// </summary>
        [JsonIgnore]
        public State CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;

                if (value != null)
                {
                    if (!States.Contains(value))
                        throw new ArgumentException("Unknown state");
                }

                _currentState = value;
                StateChanged?.Invoke(this);
            }
        }

        public State[] States { get; set; } = NoStates;

        public event Action<VisualObject> StateChanged;

        /// <summary>
        /// Current angle (degrees)
        /// </summary>
        [JsonIgnore]
        public int CurrentAngle
        {
            get => _currentAngle;
            set
            {
                var a = value;
                while (a > 180)
                    a -= 360;
                while (a <= -180)
                    a += 360;

                if (a == _currentAngle)
                    return;

                _currentAngle = a;
                AngleChanged?.Invoke(this);
            }
        }

        public event Action<VisualObject> AngleChanged;

        public void Add(State state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (States.Contains(state))
                throw new Exception("State already exist");

            if (string.IsNullOrWhiteSpace(state.Name))
                throw new Exception("Name is empty");

            var list = States.ToList();
            list.Add(state);
            States = list.ToArray();
        }

        public void Remove(State state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            if (!States.Contains(state))
                throw new Exception("State not extst");

            var list = States.ToList();
            list.Remove(state);
            States = list.ToArray();
        }
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

        public void Add(View view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            if (Views.Contains(view))
                throw new ArgumentException("View already exist");

            if (Views.Any(v => v.Angle == view.Angle))
                throw new ArgumentException("This angle already exist");

            var list = Views.ToList();
            list.Add(view);
            Views = list.ToArray();
        }

        public void Remove(View view)
        {
            if (view == null) throw new ArgumentNullException(nameof(view));

            if (!Views.Contains(view))
                throw new ArgumentException("View not found");

            var list = Views.ToList();
            list.Remove(view);
            Views = list.ToArray();
        }
    }

    public class StateSound
    {
        public byte[] RawData { get; set; }
    }
}
