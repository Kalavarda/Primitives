using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.Skills
{
    public interface IUsable
    {
        IProcess Use(ISkilled actor);
    }
}
