using System;
using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.Skills
{
    public interface ISkill
    {
        string Name { get; }

        float MaxDistance { get; }

        ITimeLimiter TimeLimiter { get; }

        IProcess Use(ISkilled initializer);
    }

    public interface IHasKey
    {
        string Key { get; }
    }

    public interface IHasCount<T> where T: struct
    {
        T Count { get; }

        T? Max { get; }

        event Action<IHasCount<T>> CountChanged;
    }
}
