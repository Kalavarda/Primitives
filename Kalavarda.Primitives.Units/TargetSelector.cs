namespace Kalavarda.Primitives.Units
{
    public class TargetSelector
    {
        private readonly Unit _hero;
        private readonly Map _map;

        public float MaxSelectionDistance { get; }

        public TargetSelector(Unit hero, Map map, float maxSelectionDistance)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _map = map ?? throw new ArgumentNullException(nameof(map));
            MaxSelectionDistance = maxSelectionDistance;
        }

        public Unit Select()
        {
            var dict = new Dictionary<Unit, float>();

            foreach (var unit in _map.Layers.Where(l => !l.IsHidden).SelectMany(l => l.Objects).OfType<Unit>())
            {
                var distance = _hero.Position.DistanceTo(unit.Position);
                if (distance > MaxSelectionDistance)
                    continue;

                var angle = _hero.Position.AngleTo(unit.Position);
                var angleDiff = angle - _hero.MoveDirection.Value;
                while (angleDiff > MathF.PI)
                    angleDiff -= 2 * MathF.PI;
                while (angleDiff < -MathF.PI)
                    angleDiff += 2 * MathF.PI;
                angleDiff = MathF.Abs(angleDiff);

                dict.Add(unit, distance * distance * angleDiff);
            }

            if (dict.Count == 0)
                return null;

            var bestTarget = dict.MinBy(p => p.Value).Key;

            if (_hero.Target == bestTarget)
            {
                dict.Remove(bestTarget);
                if (dict.Count == 0)
                    return null;
                bestTarget = dict.MinBy(p => p.Value).Key;
            }

            return bestTarget;
        }
    }
}
