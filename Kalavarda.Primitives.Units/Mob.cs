using System.Collections.Immutable;
using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.Units
{
    public abstract class Mob : Unit
    {
        private static readonly ICollection<Mob> _mobs = new List<Mob>();

        private MobState _state = MobState.New;

        public static IReadOnlyCollection<Mob> Mobs
        {
            get
            {
                lock(_mobs)
                    return _mobs.ToImmutableArray();
            }
        }

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

        public Spawn Spawn { get; }

        protected Mob(RangeF moveSpeed, Spawn spawn) : base(moveSpeed)
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
    }
}
