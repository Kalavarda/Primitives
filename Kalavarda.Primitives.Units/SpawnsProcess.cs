using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.Units
{
    public class SpawnsProcess: IProcess
    {
        private readonly Map _map;
        private readonly MapLayer _targetLayer;
        private readonly CancellationToken _cancellationToken;

        public SpawnsProcess(Map map, MapLayer targetLayer, CancellationToken cancellationToken)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _targetLayer = targetLayer ?? throw new ArgumentNullException(nameof(targetLayer));
            _cancellationToken = cancellationToken;
        }

        public event Action<IProcess> Completed;
        
        public void Process(TimeSpan delta)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                Stop();
                return;
            }

            foreach (var spawn in _map.Layers.SelectMany(l => l.Objects).OfType<SpawnBase>())
            {
                var unit = spawn.Create();
                if (unit != null)
                    _targetLayer.Add(unit);
            }
        }

        public void Stop()
        {
            Completed?.Invoke(this);
        }
    }
}
