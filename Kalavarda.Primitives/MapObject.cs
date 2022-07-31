using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;

namespace Kalavarda.Primitives
{
    public class MapObject: IMapObject, IHasImage
    {
        public PointF Position { get; }
        
        public MapObjectType Type { get; }

        public BoundsF Bounds { get; }

        public MapObject(PointF position, MapObjectType type)
        {
            Position = position;
            Type = type;
            Bounds = new RoundBounds(position, type.Radius);
        }

        public Uri ImageUri => Type.ImageUri;
        public virtual void Dispose()
        {
            Disposing?.Invoke(this);
        }

        public event Action<IMapObject> Disposing;
    }

    public class MapObjectType: IHasImage
    {
        public MapObjectType(Uri imageUri, float radius)
        {
            ImageUri = imageUri;
            Radius = radius;
        }

        public float Radius { get; }

        public Uri ImageUri { get; }
    }
}
