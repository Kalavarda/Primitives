using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Sound;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units.EventAggregators
{
    public class MapEventAggregator: IDisposable, ICreatureEvents, IMakeSounds, ISkillReceiver, ISelectableEvents
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

        public event Action<IFighter, IFighter> NegativeSkillReceived;

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
            if (mapObject is Mob mob)
                mob.IsSelectableChanged += Mob_IsSelectableChanged;
        }

        private void Mob_IsSelectableChanged(ISelectable selectable)
        {
            IsSelectableChanged?.Invoke(selectable);
        }

        private void Unit_NegativeSkillReceived(IFighter fromUnit, IFighter toUnit)
        {
            NegativeSkillReceived?.Invoke(fromUnit, toUnit);
        }

        private void Mob_PlaySound(string soundKey)
        {
            PlaySound?.Invoke(soundKey);
        }

        private void Mob_Died(ICreature creature)
        {
            Died?.Invoke(creature);
        }

        private void Mob_Disposing(IHasDispose hasDispose)
        {
            var unit = (Unit)hasDispose;
            unit.Died -= Mob_Died;
            unit.PlaySound -= Mob_PlaySound;
            unit.NegativeSkillReceived -= Unit_NegativeSkillReceived;
            unit.Disposing -= Mob_Disposing;

            if (hasDispose is Mob mob)
                mob.IsSelectableChanged -= Mob_IsSelectableChanged;
        }

        public void Dispose()
        {
            _map.LayerAdded -= Map_LayerAdded;
            foreach (var mapLayer in _map.Layers)
            {
                mapLayer.ObjectAdded -= MapLayer_ObjectAdded;
                foreach (var mapObject in mapLayer.Objects)
                {
                    if (mapObject is Unit unit)
                    {
                        unit.Died -= Mob_Died;
                        unit.PlaySound -= Mob_PlaySound;
                        unit.NegativeSkillReceived -= Unit_NegativeSkillReceived;
                        unit.Disposing -= Mob_Disposing;
                    }
                    if (mapObject is Mob mob)
                        mob.IsSelectableChanged -= Mob_IsSelectableChanged;
                }
            }
        }

        public event Action<ISelectable> IsSelectableChanged;
    }
}
