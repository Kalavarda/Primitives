using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Units.Interfaces;

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

            if (obj is Unit unit)
                unit.Disposing += Unit_Disposing;
        }

        private void Unit_Disposing(IHasDispose hasDispose)
        {
            Remove((IMapObject)hasDispose);
            hasDispose.Disposing -= Unit_Disposing;
        }

        public void Remove(IMapObject obj)
        {
            _objects.Remove(obj);
            ObjectRemoved?.Invoke(obj);
        }

        public event Action<IMapObject> ObjectAdded;

        public event Action<IMapObject> ObjectRemoved;

        public bool IsHidden { get; set; }
    }
}
