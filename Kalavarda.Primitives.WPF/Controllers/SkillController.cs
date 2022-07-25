using System;
using System.Linq;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.WPF.Binds;

namespace Kalavarda.Primitives.WPF.Controllers
{
    public class SkillController : IDisposable
    {
        private readonly IKeyBindsController _keyBindsController;
        private readonly IProcessor _processor;
        private readonly ISkillBinds _skillBinds;
        private readonly ISkilled _hero;

        public SkillController(IKeyBindsController keyBindsController, IProcessor processor, ISkillBinds skillBinds, ISkilled hero)
        {
            _keyBindsController = keyBindsController ?? throw new ArgumentNullException(nameof(keyBindsController));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _skillBinds = skillBinds ?? throw new ArgumentNullException(nameof(skillBinds));
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));

            _keyBindsController.BindActivated += KeyBindsController_BindActivated;
        }

        private void KeyBindsController_BindActivated(KeyBind keyBind)
        {
            #region TODO: Можно закэшировать

            var skillCode = _skillBinds.GetSkillCode(keyBind.Code);
            if (string.IsNullOrEmpty(skillCode))
                return;

            var skill = _hero.Skills.FirstOrDefault(s => s.Key == skillCode);
            if (skill == null)
                return;

            #endregion

            var process = skill.Use();
            if (process != null)
                _processor.Add(process);
        }

        public void Dispose()
        {
            _keyBindsController.BindActivated += KeyBindsController_BindActivated;
        }
    }
}
