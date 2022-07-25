using Kalavarda.Primitives.Sound;

namespace Kalavarda.Primitives.Units
{
    public class SoundController: IDisposable
    {
        private readonly IMakeSounds _soundsEventAggregator;
        private readonly IMakeSounds _hero;
        private readonly ISoundPlayer _soundPlayer;

        public SoundController(IMakeSounds soundsEventAggregator, IMakeSounds hero, ISoundPlayer soundPlayer)
        {
            _soundsEventAggregator = soundsEventAggregator ?? throw new ArgumentNullException(nameof(soundsEventAggregator));
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));

            _soundsEventAggregator.PlaySound += OnPlaySound;
            _hero.PlaySound += OnPlaySound;
        }

        private void OnPlaySound(string soundKey)
        {
            _soundPlayer.Play(soundKey);
        }

        public void Dispose()
        {
            _soundsEventAggregator.PlaySound -= OnPlaySound;
            _hero.PlaySound -= OnPlaySound;
        }
    }
}
