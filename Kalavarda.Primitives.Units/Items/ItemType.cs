using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units.Items
{
    public class ItemType: IHasName, IHasId, IHasImage
    {
        public uint Id { get; }

        public string Name { get; }

        public ItemType(uint id, string name, ItemQuality quality)
        {
            Id = id;
            Name = name;
            Quality = quality;
        }

        //public TimeSpan UseInterval { get; }

        //public ushort? RequiredLevel { get; }

        public ItemQuality Quality { get; }
        
        public Uri ImageUri { get; set; }
    }
}
