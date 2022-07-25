using System.Timers;
using Kalavarda.Primitives.Abstract;
using Timer = System.Timers.Timer;

namespace Kalavarda.Primitives.Units.Fight
{
    public interface IFightController
    {
        IReadOnlyCollection<Fight> Fights { get; }

        Fight CurrentFight { get; }

        event Action CurrentFightChanged;
    }

    public class FightController : IDisposable, IFightController
    {
        private readonly TimeSpan FightExitDuration = TimeSpan.FromSeconds(5);

        private readonly Map _map;
        private readonly Unit _hero;
        private DateTime _lastNegativeReceivedTime = DateTime.MinValue;
        private readonly Timer _timer = new(TimeSpan.FromSeconds(1).TotalMilliseconds) { AutoReset = true };
        private readonly ICollection<Fight> _fights = new List<Fight>();
        private Fight _currentFight;

        public IReadOnlyCollection<Fight> Fights
        {
            get
            {
                lock (_fights)
                    return _fights.ToArray();
            }
        }

        public Fight CurrentFight
        {
            get => _currentFight;
            private set
            {
                if (_currentFight == value)
                    return;

                _currentFight = value;
                CurrentFightChanged?.Invoke();
            }
        }

        public event Action CurrentFightChanged;

        public FightController(Map map, Unit hero)
        {
            _map = map;
            _hero = hero;

            _hero.NegativeSkillReceived += NegativeSkillReceived;

            _map.LayerAdded += Map_LayerAdded;
            foreach (var layer in map.Layers)
                Map_LayerAdded(layer);

            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentFight != null)
                if (DateTime.Now - _lastNegativeReceivedTime > FightExitDuration)
                    CurrentFight = null;
        }

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
                unit.NegativeSkillReceived += NegativeSkillReceived;
                unit.Disposing += Unit_Disposing;
            }
        }

        private void Unit_Disposing(Unit unit)
        {
            unit.NegativeSkillReceived -= NegativeSkillReceived;
        }

        private void NegativeSkillReceived(Unit fromUnit, Unit toUnit)
        {
            _lastNegativeReceivedTime = DateTime.Now;
            if (CurrentFight == null)
            {
                CurrentFight = new Fight(fromUnit, toUnit);
                lock (_fights)
                    _fights.Add(CurrentFight);
            }
            else
            {
                CurrentFight.Add(fromUnit);
                CurrentFight.Add(toUnit);
            }
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();

            foreach (var layer in _map.Layers)
            {
                layer.ObjectAdded -= MapLayer_ObjectAdded;
                foreach (var mapObject in layer.Objects)
                    if (mapObject is Unit unit)
                        unit.NegativeSkillReceived -= NegativeSkillReceived;
            }

            _map.LayerAdded -= Map_LayerAdded;
            _hero.NegativeSkillReceived -= NegativeSkillReceived;
        }
    }
}
