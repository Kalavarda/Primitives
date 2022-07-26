namespace Kalavarda.Primitives.Units.Interfaces
{
    public interface IHasLevel
    {
        ushort Level { get; }

        event Action<IHasLevel> LevelChanged;
    }
}
