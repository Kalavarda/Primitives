namespace Kalavarda.Primitives.Units.Items
{
    public interface IItemContainer
    {
        IReadOnlyCollection<Item> Items { get; }

        event Action<IItemContainer> Changed;
    }
}
