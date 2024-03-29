﻿using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units
{
    public class LevelMultiplier : ILevelMultiplier
    {
        private readonly float _base;

        public LevelMultiplier(float baseValue)
        {
            _base = baseValue;
        }

        public float GetRatio(ushort level)
        {
            return MathF.Pow(_base, level - 1);
        }

        public float GetRatio(float power)
        {
            return MathF.Pow(_base, power);
        }

        public float GetValue(float baseValue, ushort level)
        {
            return baseValue * GetRatio(level);
        }

        public long GetValue(int baseValue, ushort level)
        {
            return (long)MathF.Round(baseValue * GetRatio(level));
        }
    }
}
