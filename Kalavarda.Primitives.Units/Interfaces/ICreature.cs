namespace Kalavarda.Primitives.Units.Interfaces
{
    public interface ICreatureEvents
    {
        event Action<ICreature> Died;
    }

    public interface ICreature: ICreatureEvents
    {
        RangeF HP { get; }

        bool IsAlive { get; }

        bool IsDead { get; }
    }
}
