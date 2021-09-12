using System;
using System.Collections.Generic;
using System.Windows.Input;
using Kalavarda.Primitives.Skills;

namespace Kalavarda.Primitives.WPF.Skills
{
    public class SkillBind
    {
        public string SkillKey { get; }

        public Key? Key { get; }

        public MouseButton? MouseButton { get; }

        public SkillBind(string skillKey, Key? key, MouseButton? mouseButton = null)
        {
            SkillKey = skillKey ?? throw new ArgumentNullException(nameof(skillKey));
            Key = key;
            MouseButton = mouseButton;
        }
    }

    public interface ISkillBinds
    {
        IReadOnlyCollection<SkillBind> SkillBinds { get; }

        ISkill GetSkill(string key);

        SkillBind GetBind(string key);
    }
}
