using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units.Aggregators
{
    public class MapEventAggregator: IDisposable, ICreatureEventAggregator
    {
        private readonly Map _map;

        public MapEventAggregator(Map map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));

            _map.LayerAdded += Map_LayerAdded;
            foreach (var mapLayer in _map.Layers)
                Map_LayerAdded(mapLayer);
        }

        public RangeF HP => throw new NotSupportedException();

        public bool IsAlive => throw new NotSupportedException();

        public bool IsDead => throw new NotSupportedException();

        public event Action<ICreature> Died;

        private void Map_LayerAdded(MapLayer mapLayer)
        {
            mapLayer.ObjectAdded += MapLayer_ObjectAdded;
            foreach (var mapObject in mapLayer.Objects)
                MapLayer_ObjectAdded(mapObject);
        }

        private void MapLayer_ObjectAdded(IMapObject mapObject)
        {
            if (mapObject is Mob mob)
            {
                mob.Died += Mob_Died;
                mob.Disposing += Mob_Disposing;
            }
        }

        private void Mob_Disposing(Unit mob)
        {
            mob.Died -= Mob_Died;
            mob.Disposing -= Mob_Disposing;
        }

        private void Mob_Died(ICreature obj)
        {
            Died?.Invoke(obj);
        }

        public void Dispose()
        {
            _map.LayerAdded -= Map_LayerAdded;
            foreach (var mapLayer in _map.Layers)
            {
                mapLayer.ObjectAdded -= MapLayer_ObjectAdded;
                foreach (var mapObject in mapLayer.Objects)
                    if (mapObject is Mob mob)
                    {
                        mob.Died -= Mob_Died;
                        mob.Disposing -= Mob_Disposing;
                    }
            }
        }
    }
}
