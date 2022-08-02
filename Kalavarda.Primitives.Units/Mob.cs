using System.Collections.Immutable;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units
{
    public abstract class Mob : Unit, IHasLevel, IMob, IChangesModifier, IFighter
    {
        private static readonly ICollection<Mob> _mobs = new List<Mob>();

        private MobState _state = MobState.New;
        private ushort _level;

        public static IReadOnlyCollection<Mob> Mobs
        {
            get
            {
                lock(_mobs)
                    return _mobs.ToImmutableArray();
            }
        }

        public float AttackRatio { get; set; }
        
        public float DefRatio { get; set; }

        public MobState State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;

                var oldValue = _state;
                _state = value;
                StateChangedTime = DateTime.Now;

                StateChanged?.Invoke(this, oldValue, value);
            }
        }

        public event Action<Mob, MobState, MobState> StateChanged;

        public DateTime StateChangedTime { get; private set; } = DateTime.Now;

        public abstract float AggrDistance { get; }

        public abstract float MaxDistanceFromSpawn { get; }

        public abstract IProcess CreateFightProcess(IProcessor processor);

        public SpawnBase Spawn { get; }

        protected Mob(RangeF moveSpeed, SpawnBase spawn) : base(moveSpeed)
        {
            lock(_mobs)
                _mobs.Add(this);
            Spawn = spawn;
        }

        public enum MobState
        {
            New,
            Idle,
            Fight,
            Dead,
            Returning,
            Removing
        }

        public static void Remove(Mob mob)
        {
            lock (_mobs)
                _mobs.Remove(mob);
        }

        public ushort Level
        {
            get => _level;
            set
            {
                if (_level == value)
                    return;

                _level = value;
                LevelChanged?.Invoke(this);
            }
        }

        public event Action<IHasLevel> LevelChanged;

        public void ChangeIncome(UnitChanges changes)
        {
            changes.HP /= DefRatio;
        }

        public void ChangeOutcome(UnitChanges changes)
        {
            changes.HP *= AttackRatio;
        }

        public string Name => GetType().Name;
    }
}
