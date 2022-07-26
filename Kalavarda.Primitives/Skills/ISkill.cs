using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.Skills
{
    public interface ISkill: IHasKey
    {
        string Name { get; }

        ITimeLimiter TimeLimiter { get; }

        IProcess Use();
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
