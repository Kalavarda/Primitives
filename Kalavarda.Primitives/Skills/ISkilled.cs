using System;
using System.Collections.Generic;
using System.Linq;

namespace Kalavarda.Primitives.Skills
{
    public interface ISkilled
    {
        IEnumerable<ISkill> Skills { get; }
    }

    public static class SkilledExtensions
    {
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
