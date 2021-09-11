using System;

namespace Kalavarda.Primitives.Geometry
{
    public class AngleF
    {
        private const double MinDiff = MathF.PI / 100;

        private float _value;

        public event Action Changed;

        /// <summary>
        /// В радианах
        /// </summary>
        public float Value
        {
            get => _value;
            set
            {
                if (MathF.Abs(_value - value) < MinDiff)
                    return;

                _value = value;

                Changed?.Invoke();
            }
        }

        /// <summary>
        /// В градусах
        /// </summary>
        public float ValueInDegrees
        {
            get => 180 * Value / MathF.PI;
            set => Value = MathF.PI * value / 180;
        }
    }
}
