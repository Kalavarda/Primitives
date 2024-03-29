﻿using System;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Skills
{
    public interface ISkill: IHasKey, IUsable
    {
        string Name { get; }

        ITimeLimiter TimeLimiter { get; }
    }

    public interface IDistanceSkill
    {
        float MinDistance { get; }

        float MaxDistance { get; }
    }

    public interface IHasCount<T> where T: struct
    {
        T Count { get; }

        T? Max { get; }

        event Action<IHasCount<T>> CountChanged;
    }
}
