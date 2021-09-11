using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalavarda.Primitives.Skills
{
    public interface ISkilled
    {
        IReadOnlyCollection<ISkill> Skills { get; }
    }

    public static class SkilledExtensions
    {
        public static float GetMaxSkillDistance(this ISkilled skilled)
        {
            var readyDistances = skilled.GetReadySkills()
                .Select(sk => sk.MaxDistance)
                .ToArray();
            return readyDistances.Any()
                ? readyDistances.Max()
                : skilled.Skills.OrderBy(sk => sk.TimeLimiter.Remain).First().MaxDistance;
        }

        /// <summary>
        /// Возвращает умения, которые не в cooldown
        /// </summary>
        public static IEnumerable<ISkill> GetReadySkills(this ISkilled skilled)
        {
            return skilled.Skills
                .Where(sk => sk.TimeLimiter.Remain == TimeSpan.Zero);
        }
    }
}
