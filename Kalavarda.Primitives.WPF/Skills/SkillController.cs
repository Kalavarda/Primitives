using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Kalavarda.Primitives.Process;

namespace Kalavarda.Primitives.WPF.Skills
{
    public class SkillController : IDisposable
    {
        private readonly IInputElement _uiElement;
        private readonly IProcessor _processor;
        private readonly ISkillBinds _skillBinds;

        public SkillController(IInputElement uiElement, IProcessor processor, ISkillBinds skillBinds)
        {
            _uiElement = uiElement ?? throw new ArgumentNullException(nameof(uiElement));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _skillBinds = skillBinds ?? throw new ArgumentNullException(nameof(skillBinds));

            _uiElement.KeyDown += UiElement_KeyDown;
            _uiElement.MouseLeftButtonDown += UiElement_MouseDown;
        }

        private void UiElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled)
                return;

            var bind = _skillBinds.SkillBinds.FirstOrDefault(sb => sb.MouseButton == e.ChangedButton);
            if (bind != null)
                UseBind(e, bind);
        }

        private void UiElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;

            var bind = _skillBinds.SkillBinds.FirstOrDefault(sb => sb.Key == e.Key);
            if (bind != null)
                UseBind(e, bind);
        }

        private void UseBind(RoutedEventArgs e, SkillBind bind)
        {
            var skill = _skillBinds.GetSkill(bind.SkillKey);
            if (skill != null)
            {
                e.Handled = true;
                var process = skill.Use();
                if (process != null)
                    _processor.Add(process);
            }
        }

        public void Dispose()
        {
            _uiElement.MouseLeftButtonDown -= UiElement_MouseDown;
            _uiElement.KeyDown -= UiElement_KeyDown;
        }
    }
}
