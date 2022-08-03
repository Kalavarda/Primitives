using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.Sound;
using Kalavarda.Primitives.Units.Buffs;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units
{
    public abstract class Unit : IMapObject, ISkilled, ICreature, IMakeSounds, ISkillReceiver, IHasBuffs, IHasDispose
    {
        public static readonly TimeSpan GlobalCooldown = TimeSpan.FromSeconds(0.5);
        private ISelectable _target;

        private static uint _counter;

        public uint Id { get; }

        public AngleF MoveDirection { get; } = new();

        public RangeF MoveSpeed { get; }

        public PointF Position { get; } = new();

        /// <summary>
        /// В какую точку нужно двигаться
        /// </summary>
        public PointF MoveTarget { get; } = new();

        protected Unit(RangeF moveSpeed)
        {
            Id = _counter++;

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
        public bool IsAlive => !IsDead;

        /// <inheritdoc/>
        public bool IsDead => HP.IsMin;

        public event Action<ICreature> Died;

        public ISelectable Target
        {
            get => _target;
            set
            {
                if (_target == value)
                    return;

                var oldValue = _target;
                _target = value;

                TargetChanged?.Invoke(oldValue, value);
            }
        }

        public event Action<ISelectable, ISelectable> TargetChanged;

        public abstract IEnumerable<ISkill> Skills { get; }

        public event Action<string> PlaySound;

        protected void RaisePlaySound(string soundKey)
        {
            PlaySound?.Invoke(soundKey);
        }

        public event Action<IHasDispose> Disposing;

        public static void Apply(IFighter from, UnitChanges changes, IFighter target)
        {
            if (from is IChangesModifier modifierF)
                modifierF.ChangeOutcome(changes);

            if (target is IChangesModifier modifierT)
                modifierT.ChangeIncome(changes);

            ChangeHP(from, target, changes.HP);
        }

        private static void ChangeHP(IFighter from, IFighter target, float hpDelta)
        {
            if (target is ICreature creature)
            {
                var oldHp = creature.HP.Value;

                creature.HP.Value += hpDelta;

                if (creature.HP.Value < oldHp)
                    if (target is Unit unit)
                        unit.NegativeSkillReceived?.Invoke(from, target);
            }
            else
                throw new NotImplementedException();
        }

        public event Action<IFighter, IFighter> NegativeSkillReceived;

        public void Dispose()
        {
            Disposing?.Invoke(this);
        }

        private readonly ICollection<Buff> _buffs = new List<Buff>();

        public IReadOnlyCollection<Buff> Buffs
        {
            get
            {
                lock (_buffs)
                    return _buffs.ToArray();
            }
        }

        public event Action<IReadonlyHasBuffs, Buff> BuffAdded;
        public event Action<IReadonlyHasBuffs, Buff> BuffRemoved;

        public void Add(Buff buff)
        {
            lock(_buffs)
                _buffs.Add(buff);
            BuffAdded?.Invoke(this, buff);
        }

        public void Remove(Buff buff)
        {
            lock (_buffs)
                _buffs.Remove(buff);
            BuffRemoved?.Invoke(this, buff);
        }
    }
}