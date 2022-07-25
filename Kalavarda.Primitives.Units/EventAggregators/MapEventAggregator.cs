using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Sound;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units.EventAggregators
{
    public class MapEventAggregator: IDisposable, ICreatureEventAggregator, IMakeSounds, ISkillReceiver
    {
        private readonly Map _map;

        public MapEventAggregator(Map map)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));

            _map.LayerAdded += Map_LayerAdded;
            foreach (var mapLayer in _map.Layers)
                Map_LayerAdded(mapLayer);
        }

        public event Action<ICreature> Died;

        public event Action<string> PlaySound;

        public event Action<Unit, Unit> NegativeSkillReceived;

        private void Map_LayerAdded(MapLayer mapLayer)
        {
            mapLayer.ObjectAdded += MapLayer_ObjectAdded;
            foreach (var mapObject in mapLayer.Objects)
                MapLayer_ObjectAdded(mapObject);
        }

        private void MapLayer_ObjectAdded(IMapObject mapObject)
        {
            if (mapObject is Unit unit)
            {
                unit.Died += Mob_Died;
                unit.PlaySound += Mob_PlaySound;
                unit.NegativeSkillReceived += Unit_NegativeSkillReceived;
                unit.Disposing += Mob_Disposing;
            }
        }

        private void Unit_NegativeSkillReceived(Unit fromUnit, Unit toUnit)
        {
            NegativeSkillReceived?.Invoke(fromUnit, toUnit);
        }

        private void Mob_PlaySound(string soundKey)
        {
            PlaySound?.Invoke(soundKey);
        }

        private void Mob_Died(ICreature obj)
        {
            Died?.Invoke(obj);
        }

        private void Mob_Disposing(Unit mob)
        {
            mob.Died -= Mob_Died;
            mob.PlaySound -= Mob_PlaySound;
            mob.NegativeSkillReceived -= Unit_NegativeSkillReceived;
            mob.Disposing -= Mob_Disposing;
        }

        public void Dispose()
        {
            _map.LayerAdded -= Map_LayerAdded;
            foreach (var mapLayer in _map.Layers)
            {
                mapLayer.ObjectAdded -= MapLayer_ObjectAdded;
                foreach (var mapObject in mapLayer.Objects)
                    if (mapObject is Unit unit)
                    {
                        unit.Died -= Mob_Died;
                        unit.PlaySound -= Mob_PlaySound;
                        unit.NegativeSkillReceived -= Unit_NegativeSkillReceived;
                        unit.Disposing -= Mob_Disposing;
                    }
            }
        }
    }
}
