using System.Diagnostics;
using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units.Items
{
    [DebuggerDisplay("{Name} x{Count}")]
    public class Item: IHasId, IHasName, IHasImage, IHasCount
    {
        private static uint _counter;
        
        private uint _count = 1;

        public ItemType Type { get; }

        public Item(ItemType type)
        {
            _counter++;
            Id = _counter;

            Type = type;
        }

        public uint Id { get; }

        public string Name => Type.Name;
        
        public Uri ImageUri => Type.ImageUri;

        public uint Count
        {
            get => _count;
            set
            {
                if (_count == value)
                    return;

                var oldValue = _count;
                _count = value;
                CountChanged?.Invoke(this, oldValue, value);
            }
        }

        public event Action<IHasCount, uint, uint> CountChanged;

        public Item Clone()
        {
            return new Item(Type) { Count =Count };
        }
    }
}
