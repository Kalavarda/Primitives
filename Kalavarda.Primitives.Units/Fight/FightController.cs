using System.Timers;
using Kalavarda.Primitives.Units.Interfaces;
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

        private readonly ISkillReceiver _skillReceiveAggregator;
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

        public FightController(ISkillReceiver skillReceiveAggregator, Unit hero)
        {
            _skillReceiveAggregator = skillReceiveAggregator ?? throw new ArgumentNullException(nameof(skillReceiveAggregator));
            _hero = hero;

            _hero.NegativeSkillReceived += NegativeSkillReceived;
            _skillReceiveAggregator.NegativeSkillReceived += NegativeSkillReceived;

            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (CurrentFight != null)
                if (DateTime.Now - _lastNegativeReceivedTime > FightExitDuration)
                    CurrentFight = null;
        }

        private void NegativeSkillReceived(IFighter fromUnit, IFighter toUnit)
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

            _skillReceiveAggregator.NegativeSkillReceived -= NegativeSkillReceived;
            _hero.NegativeSkillReceived -= NegativeSkillReceived;
        }
    }
}
