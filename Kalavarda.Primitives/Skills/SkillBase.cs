﻿using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.Skills
{
    public abstract class SkillBase: ISkill
    {
        private readonly TimeLimiter _timeLimiter;
        private readonly ISkillProcessFactory _processFactory;

        public abstract string Name { get; }

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public float MaxDistance { get; }

        protected SkillBase(float maxDistance, TimeSpan interval, ISkillProcessFactory processFactory, TimeSpan? firstTimeDelay = null)
        {
            MaxDistance = maxDistance;
            _timeLimiter = firstTimeDelay != null ? new TimeLimiter(interval, firstTimeDelay.Value) : new TimeLimiter(interval);
            _processFactory = processFactory ?? throw new ArgumentNullException(nameof(processFactory));
        }

        public IProcess Use(ISkilled initializer)
        {
            IProcess skillProcess = null;
            _timeLimiter.Do(() =>
            {
                if (initializer is ICreature creature)
                    if (creature.IsDead)
                        return;

                skillProcess = _processFactory.Create(initializer, this);
            });
            return skillProcess;
        }
    }
}