using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Skills;

namespace Kalavarda.Primitives.Units
{
    public abstract class Unit : IMapObject, ISkilled, ICreature
    {
        public static readonly TimeSpan GlobalCooldown = TimeSpan.FromSeconds(0.5);

        public AngleF MoveDirection { get; } = new();

        public RangeF MoveSpeed { get; }

        public PointF Position { get; } = new();

        /// <summary>
        /// В какую точку нужно двигаться
        /// </summary>
        public PointF MoveTarget { get; } = new();

        protected Unit(RangeF moveSpeed)
        {
            MoveSpeed = moveSpeed;
            HP.ValueMin += HP_ValueMin;
        }

        private void HP_ValueMin(RangeF obj)
        {
            Died?.Invoke(this);
        }

        public abstract BoundsF Bounds { get; }

        public RangeF HP { get; } = new();

        /// <inheritdoc/>
        public bool IsAlive => !HP.IsMin;

        /// <inheritdoc/>
        public bool IsDead => HP.IsMin;

        public event Action<ICreature> Died;

        public Unit Target { get; set; }

        public abstract IEnumerable<ISkill> Skills { get; }
    }
}