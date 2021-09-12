using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Kalavarda.Primitives
{
    [DebuggerDisplay("{Value} (min={Min}; max={Max})")]
    public class RangeF
    {
        private const float DeltaF = 0.0001f;

        private float _value;
        private float _max;
        private float _min;

        public float Min
        {
            get => _min;
            set
            {
                if (value > Max)
                    throw new ArgumentException();

                if (MathF.Abs(value - _min) < DeltaF)
                    return;

                _min = value;

                if (Value < _min)
                    Value = _min;

                MinChanged?.Invoke(this);
            }
        }

        public float Max
        {
            get => _max;
            set
            {
                if (value < Min)
                    throw new ArgumentException();

                if (MathF.Abs(value - _max) < DeltaF)
                    return;

                _max = value;

                if (Value > _max)
                    Value = _max;

                MaxChanged?.Invoke(this);
            }
        }

        public float Value
        {
            get => _value;
            set
            {
                if (value > Max)
                    value = Max;

                if (value < Min)
                    value = Min;

                if (MathF.Abs(_value - value) < DeltaF)
                    return;

                _value = value;
                ValueChanged?.Invoke(this);

                if (MathF.Abs(_value - _min) < DeltaF)
                    ValueMin?.Invoke(this);

                if (MathF.Abs(_value - _max) < DeltaF)
                    ValueMax?.Invoke(this);
            }
        }

        /// <summary>
        /// NormalizedValue
        /// </summary>
        [JsonIgnore]
        public float ValueN => (_value - _min) / (_max - _min);

        public event Action<RangeF> ValueChanged;

        public event Action<RangeF> MinChanged;
        public event Action<RangeF> MaxChanged;

        public event Action<RangeF> ValueMin;
        public event Action<RangeF> ValueMax;

        public RangeF()
        {
        }

        public RangeF(float min, float max): this()
        {
            Min = min;
            Max = max;
        }

        public void SetMin()
        {
            Value = Min;
        }

        public void SetMax()
        {
            Value = Max;
        }

        [JsonIgnore]
        public bool IsMin => MathF.Abs(Value - Min) < DeltaF;

        [JsonIgnore]
        public bool IsMax => MathF.Abs(Max - Value) < DeltaF;
    }
}
