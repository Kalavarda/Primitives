using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.Skills
{
    public interface ISkillProcessFactory
    {
        IProcess Create(ISkilled initializer, ISkill skill);
    }
}
