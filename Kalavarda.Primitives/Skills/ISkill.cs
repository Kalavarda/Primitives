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
}
