namespace Kalavarda.Primitives.Units.Items
{
    public interface IReadonlyItemContainer
    {
        IReadOnlyCollection<Item> Items { get; }

        event Action<IItemContainer> Changed;
    }

    public interface IItemContainer: IReadonlyItemContainer
    {
        void Add(Item item);

        bool TryPull(ItemType type, uint count, out Item item);
    }
}
