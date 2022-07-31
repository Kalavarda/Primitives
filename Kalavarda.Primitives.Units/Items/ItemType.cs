using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units.Items
{
    public class ItemType: IHasName, IHasId, IHasImage
    {
        private static uint _counter;

        public uint Id { get; }

        public string Name { get; }

        public ItemType(string name, ItemQuality quality)
        {
            _counter++;
            Id = _counter;
            Name = name;
            Quality = quality;
        }

        //public TimeSpan UseInterval { get; }

        //public ushort? RequiredLevel { get; }

        public ItemQuality Quality { get; }
        
        public Uri ImageUri { get; set; }
    }
}
