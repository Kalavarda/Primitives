using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units
{
    public class MobFightProcess: IProcess
    {
        private readonly Mob _mob;
        private readonly IProcessor _processor;
        public event Action<IProcess> Completed;

        private readonly TimeLimiter _attackLimiter = new(Unit.GlobalCooldown);

        public MobFightProcess(Mob mob, IProcessor processor)
        {
            _mob = mob ?? throw new ArgumentNullException(nameof(mob));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public void Process(TimeSpan delta)
        {
            if (_mob.State != Mob.MobState.Fight)
            {
                Stop();
                return;
            }

            if (_mob.Target == null)
            {
                Stop();
                return;
            }

            if (_mob.Target is ICreature { IsDead: true })
            {
                _mob.Target = null;
                Stop();
                return;
            }

            var skillFound = false;
            _attackLimiter.Do(() =>
            {
                if (_mob.Target is IHasPosition hasPosition)
                {
                    var distance = _mob.Position.DistanceTo(hasPosition.Position);
                    foreach (var skill in _mob.GetReadySkills())
                    {
                        if (skill is IDistanceSkill distanceSkill)
                            if (distanceSkill.MaxDistance < distance)
                                continue;

                        var skillProcess = skill.Use(_mob);
                        skillFound = true;
                        if (skillProcess != null)
                            _processor.Add(skillProcess);
                        break;
                    }
                }
            });

            if (!skillFound)
                MoveToTarget(delta);
        }

        private void MoveToTarget(TimeSpan delta)
        {
            if (_mob.Target is IHasPosition hasPosition)
            {
                var distance = _mob.Position.DistanceTo(hasPosition.Position);
                var nearestSkill = _mob.Skills.OfType<IDistanceSkill>().Min(sk => sk.MaxDistance);
                if (distance < nearestSkill)
                    return;

                var angle = _mob.Position.AngleTo(hasPosition.Position);
                var d = _mob.MoveSpeed.Max * (float)delta.TotalSeconds;
                var dx = d * MathF.Cos(angle);
                var dy = d * MathF.Sin(angle);
                _mob.Position.Set(_mob.Position.X + dx, _mob.Position.Y + dy);
            }
        }

        public void Stop()
        {
            Completed?.Invoke(this);
        }
    }
}
