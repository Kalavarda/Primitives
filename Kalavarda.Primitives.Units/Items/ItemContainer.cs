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

        /// <summary>
        /// Пытается забрать указанное кол-во предметов
        /// </summary>
        public bool TryPull(ItemType type, uint count, out Item item)
        {
            item = null;

            lock (_items)
            {
                var existItem = _items.FirstOrDefault(i => i.Type == type);
                if (existItem == null || existItem.Count < count)
                    return false;

                if (existItem.Count == count)
                {
                    item = existItem;
                    _items.Remove(existItem);
                }
                else
                {
                    item = existItem.Clone();
                    item.Count = count;
                    existItem.Count -= count;
                }
                Changed?.Invoke(this);

                return true;
            }
        }
    }
}
