namespace Kalavarda.Primitives.Units.Items
{
    public class ItemContainer : IItemContainer
    {
        private readonly ICollection<Item> _items = new List<Item>();

        public IReadOnlyCollection<Item> Items
        {
            get
            {
                lock(_items)
                    return _items.ToArray();
            }
        }

        public void Add(Item item)
        {
            lock (_items)
            {
                var existItem = _items.FirstOrDefault(i => i.Type == item.Type);
                if (existItem != null)
                    existItem.Count += item.Count;
                else
                    _items.Add(item);
            }

            Changed?.Invoke(this);
        }

        public event Action<IItemContainer> Changed;
    }
}
