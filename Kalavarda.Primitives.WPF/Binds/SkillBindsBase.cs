using System.Collections.Generic;

namespace Kalavarda.Primitives.WPF.Binds
{
    public abstract class SkillBindsBase : ISkillBinds
    {
        public abstract IReadOnlyDictionary<string, string> Binds { get; }

        public string GetSkillCode(string bindCode)
        {
            return Binds.TryGetValue(bindCode, out var skillCode)
                ? skillCode
                : null;
        }
    }
}
