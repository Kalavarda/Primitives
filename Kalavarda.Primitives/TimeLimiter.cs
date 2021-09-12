using System;

namespace Kalavarda.Primitives
{
    public interface ITimeLimiter
    {
        TimeSpan Interval { get; set; }

        TimeSpan Remain { get; }
    }

    public class TimeLimiter : ITimeLimiter
    {
        private DateTime _lastTime;

        public TimeSpan Interval { get; set; }

        public TimeSpan Remain
        {
            get
            {
                var nextTime = _lastTime + Interval;
                return nextTime <= DateTime.Now
                    ? TimeSpan.Zero
                    : nextTime - DateTime.Now;
            }
        }

        public TimeLimiter(TimeSpan interval): this(interval, TimeSpan.Zero)
        {
            Interval = interval;
        }

        public TimeLimiter(TimeSpan interval, TimeSpan firstTimeDelay)
        {
            Interval = interval;
            _lastTime = DateTime.Now + firstTimeDelay - interval;
        }

        public void Do(Action action)
        {
            if (DateTime.Now - _lastTime < Interval)
                return;

            action();
            _lastTime = DateTime.Now;
        }

        public T Do<T>(Func<T> action)
        {
            if (DateTime.Now - _lastTime < Interval)
                return default;

            var result = action();
            _lastTime = DateTime.Now;
            return result;
        }
    }
}
