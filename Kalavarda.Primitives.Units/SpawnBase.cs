using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units
{
    public abstract class SpawnBase : IMapObject
    {
        private static readonly Random Random = new();

        public PointF Position { get; } = new();

        public float Radius => ((RoundBounds)Bounds).Radius;

        public ushort Count { get; private set; }

        public ushort MaxCount { get; }

        public DateTime DisposeTimestamp { get; private set; } = DateTime.MinValue;

        public TimeSpan WaitAfterDispose { get; }

        private readonly TimeLimiter _timeLimiter = new(TimeSpan.FromSeconds(1));

        protected SpawnBase(float radius, TimeSpan waitAfterDispose, ushort maxCount)
        {
            Bounds = new RoundBounds(Position, radius);
            WaitAfterDispose = waitAfterDispose;
            MaxCount = maxCount;
        }

        public IMapObject Create()
        {
            IMapObject mapObject = null;

            if (Count < MaxCount)
                _timeLimiter.Do(() =>
                {
                    if (DateTime.Now - DisposeTimestamp < WaitAfterDispose)
                        return;

                    mapObject = CreateMapObject();

                    var angle = Random.NextSingle() * 2 * MathF.PI;
                    var r = Random.NextSingle() * Radius;
                    var dx = r * MathF.Cos(angle);
                    var dy = r * MathF.Sin(angle);
                    mapObject.Position.Set(Position.X + dx, Position.Y + dy);

                    Count++;
                    if (mapObject is IHasDispose hasDispose)
                        hasDispose.Disposing += HasDispose_Disposing;
                });

            return mapObject;
        }

        private void HasDispose_Disposing(IHasDispose hasDispose)
        {
            hasDispose.Disposing -= HasDispose_Disposing;
            Count--;
            DisposeTimestamp = DateTime.Now;
        }

        protected abstract IMapObject CreateMapObject();

        public BoundsF Bounds { get; }
    }
}
