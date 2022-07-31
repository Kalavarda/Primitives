namespace Kalavarda.Primitives.Units.Items
{
    public interface IReadonlyItemsRepository
    {
        ItemType GetById(uint id);
    }
}
