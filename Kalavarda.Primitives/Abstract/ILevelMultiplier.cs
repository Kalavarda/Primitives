﻿namespace Kalavarda.Primitives.Abstract
{
    public interface ILevelMultiplier
    {
        float GetRatio(ushort level);

        float GetValue(float baseValue, ushort level);

        long GetValue(int baseValue, ushort level);
    }
}
