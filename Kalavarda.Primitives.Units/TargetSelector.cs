using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Units.Fight;

namespace Kalavarda.Primitives.Units
{
    public interface ITargetSelector
    {
        Unit Select();
    }

    public class TargetSelector : ITargetSelector
    {
        private readonly Unit _hero;
        private readonly Map _map;
        private readonly IFightController _fightController;
        private readonly IMousePositionDetector _mousePositionDetector;

        public float MaxSelectionDistance { get; }

        public TargetSelector(Unit hero, Map map, float maxSelectionDistance, IFightController fightController, IMousePositionDetector mousePositionDetector)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            _fightController = fightController ?? throw new ArgumentNullException(nameof(fightController));
            _mousePositionDetector = mousePositionDetector ?? throw new ArgumentNullException(nameof(mousePositionDetector));
            MaxSelectionDistance = maxSelectionDistance;
        }

        public Unit Select()
        {
            var dict = new Dictionary<Unit, float>();

            var (x, y) = _mousePositionDetector.GetPosition();
            var mouseAngle = MathF.Atan2(y - _hero.Position.Y, x - _hero.Position.X);

            foreach (var unit in _map.Layers.Where(l => !l.IsHidden).SelectMany(l => l.Objects).OfType<Unit>())
            {
                var distance = _hero.Position.DistanceTo(unit.Position);
                if (distance > MaxSelectionDistance)
                    continue;

                if (unit.IsDead)
                    continue;

                var angle = _hero.Position.AngleTo(unit.Position);
                var angleDiff = angle - mouseAngle;
                while (angleDiff > MathF.PI)
                    angleDiff -= 2 * MathF.PI;
                while (angleDiff < -MathF.PI)
                    angleDiff += 2 * MathF.PI;
                angleDiff = MathF.PI - MathF.Abs(angleDiff);

                var value = angleDiff * (MaxSelectionDistance - distance);
                if (_fightController.CurrentFight != null)
                    if (_fightController.CurrentFight.Members.Any(m => m.Id == unit.Id))
                        value *= 2;
                dict.Add(unit, value);
            }

            if (dict.Count == 0)
                return null;

            var bestTarget = dict.MaxBy(p => p.Value).Key;

            if (_hero.Target == bestTarget)
            {
                dict.Remove(bestTarget);
                if (dict.Count == 0)
                    return null;
                bestTarget = dict.MaxBy(p => p.Value).Key;
            }

            return bestTarget;
        }
    }
}
