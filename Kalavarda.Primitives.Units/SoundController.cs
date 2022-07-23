using Kalavarda.Primitives.Sound;

namespace Kalavarda.Primitives.Units
{
    public class SoundController: IDisposable
    {
        private readonly Map _map;
        private readonly IMakeSounds _hero;
        private readonly ISoundPlayer _soundPlayer;

        public SoundController(Map map, IMakeSounds hero, ISoundPlayer soundPlayer)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));

            map.LayerAdded += Map_LayerAdded;
            foreach (var mapLayer in map.Layers)
                Map_LayerAdded(mapLayer);

            _hero.PlaySound += OnPlaySound;
        }

        private void Map_LayerAdded(MapLayer mapLayer)
        {
            if (mapLayer.IsHidden)
                return;

            mapLayer.ObjectAdded += MapLayer_ObjectAdded;
            foreach (var obj in mapLayer.Objects)
                MapLayer_ObjectAdded(obj);
        }

        private void MapLayer_ObjectAdded(Abstract.IMapObject obj)
        {
            if (obj is IMakeSounds makeSounds)
                makeSounds.PlaySound += OnPlaySound;
        }

        private void OnPlaySound(string soundKey)
        {
            _soundPlayer.Play(soundKey);
        }

        public void Dispose()
        {
            _map.LayerAdded -= Map_LayerAdded;
            foreach (var mapLayer in _map.Layers)
            {
                mapLayer.ObjectAdded -= MapLayer_ObjectAdded;
                foreach (var obj in mapLayer.Objects)
                    if (obj is IMakeSounds makeSounds)
                        makeSounds.PlaySound -= OnPlaySound;
            }

            _hero.PlaySound -= OnPlaySound;
        }
    }
}
