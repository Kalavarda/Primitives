namespace Kalavarda.Primitives.Units.Interfaces
{
    public interface ICreature
    {
        RangeF HP { get; }

        bool IsAlive { get; }

        bool IsDead { get; }

        event Action<ICreature> Died;
    }
}
