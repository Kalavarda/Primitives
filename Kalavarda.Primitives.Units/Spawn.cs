using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;

namespace Kalavarda.Primitives.Units
{
    public abstract class Spawn : IMapObject
    {
        private static readonly Random Random = new();

        public PointF Position { get; } = new();

        public float Radius => ((RoundBounds)Bounds).Radius;

        public ushort CountOfLiveUnits { get; private set; }

        public ushort MaxOfLiveMobs  => 1;

        public DateTime LastUnitDied { get; private set; } = DateTime.MinValue;

        public TimeSpan PeriodAfterDeath { get; }

        private readonly TimeLimiter _timeLimiter = new(TimeSpan.FromSeconds(1));

        public Spawn(float radius, TimeSpan periodAfterDeath)
        {
            Bounds = new RoundBounds(Position, radius);
            PeriodAfterDeath = periodAfterDeath;
        }

        public Unit Create()
        {
            Unit unit = null;

            if (CountOfLiveUnits < MaxOfLiveMobs)
                _timeLimiter.Do(() =>
                {
                    if (DateTime.Now - LastUnitDied < PeriodAfterDeath)
                        return;

                    unit = CreateUnit();

                    var angle = Random.NextSingle() * 2 * MathF.PI;
                    var r = Random.NextSingle() * Radius;
                    var dx = r * MathF.Cos(angle);
                    var dy = r * MathF.Sin(angle);
                    unit.Position.Set(Position.X + dx, Position.Y + dy);

                    CountOfLiveUnits++;
                    unit.Died += Unit_Died;
                });

            return unit;
        }

        private void Unit_Died(ICreature creature)
        {
            creature.Died -= Unit_Died;
            CountOfLiveUnits--;
            LastUnitDied = DateTime.Now;
        }

        protected abstract Unit CreateUnit();

        public BoundsF Bounds { get; }
    }
}
