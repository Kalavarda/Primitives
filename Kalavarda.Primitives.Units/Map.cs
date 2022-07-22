using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units
{
    public class Map
    {
        private readonly ICollection<MapLayer> _layers = new List<MapLayer>();

        public IEnumerable<MapLayer> Layers => _layers;

        public void Add(MapLayer mapLayer)
        {
            _layers.Add(mapLayer);
            LayerAdded?.Invoke(mapLayer);
        }

        public event Action<MapLayer> LayerAdded;
    }

    public class MapLayer
    {
        private readonly ICollection<IMapObject> _objects = new List<IMapObject>();

        public IEnumerable<IMapObject> Objects => _objects;

        public void Add(IMapObject obj)
        {
            _objects.Add(obj);
            ObjectAdded?.Invoke(obj);
        }

        public event Action<IMapObject> ObjectAdded;

        public bool IsHidden { get; set; }
    }
}
