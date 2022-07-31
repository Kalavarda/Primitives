using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units.Buffs
{
    public class Buff: IHasName, IHasImage
    {
        public BuffType Type { get; }

        public Buff(BuffType type)
        {
            Type = type;
        }

        public string Name => Type.Name;

        public Uri ImageUri => Type.ImageUri;
    }

    public class BuffType: IHasName, IHasImage, IHasId
    {
        public BuffType(uint id, string name, Uri imageUri)
        {
            Id = id;
            Name = name;
            ImageUri = imageUri ?? throw new ArgumentNullException(nameof(imageUri));
        }

        public string Name { get; }
        
        public Uri ImageUri { get; }
        
        public uint Id { get; }
    }
}
