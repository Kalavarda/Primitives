using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units
{
    // TODO: by mouse position
    public class TargetSelector : ITargetSelector
    {
        private readonly Map _map;
        private readonly IMousePositionDetector _mousePositionDetector;

        public float MaxSelectionDistance { get; }

        public TargetSelector(Map map, float maxSelectionDistance, IMousePositionDetector mousePositionDetector)
        {
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _mousePositionDetector = mousePositionDetector ?? throw new ArgumentNullException(nameof(mousePositionDetector));
            MaxSelectionDistance = maxSelectionDistance;
        }

        public ISelectable Select(bool fightersOnly)
        {
            var dict = new Dictionary<ISelectable, float>();
            var (x, y) = _mousePositionDetector.GetPosition();

            foreach (var unit in _map.Layers.Where(l => !l.IsHidden).SelectMany(l => l.Objects).OfType<ISelectable>().Where(s => s.IsSelectable))
            {
                if (unit is IHasPosition hasPosition)
                {
                    var distance = hasPosition.Position.DistanceTo(x, y);
                    if (distance < MaxSelectionDistance)
                        dict.Add(unit, distance);
                }
            }

            return !dict.Any()
                ? null
                : dict.MinBy(p => p.Value).Key;
        }
    }
}
