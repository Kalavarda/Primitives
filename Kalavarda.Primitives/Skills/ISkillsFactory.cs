using System.Collections.Generic;

namespace Kalavarda.Primitives.Skills
{
    public interface ISkillsFactory
    {
        IReadOnlyCollection<ISkill> Create(ISkilled skilled);
    }
}
