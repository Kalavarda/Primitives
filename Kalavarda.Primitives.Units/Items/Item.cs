using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units.Items
{
    public class Item: IHasId, IHasName, IHasImage, IHasCount
    {
        private static uint _count;

        public ItemType Type { get; }

        public Item(ItemType type, uint count = 1)
        {
            _count++;
            Id = _count;

            Type = type;
            Count = count;
        }

        public uint Id { get; }

        public string Name => Type.Name;
        
        public Uri ImageUri => Type.ImageUri;
        
        public uint Count { get; }
    }
}
