using System;
using System.Windows.Input;

namespace Kalavarda.Primitives.WPF.Binds
{
    public class KeyBind
    {
        public string Code { get; }

        public string Name { get; }

        public Key? Key { get; }

        public MouseButton? MouseButton { get; }

        // TODO: + альтернативные кнопки

        public KeyBind(string code, string name, Key? key, MouseButton? mouseButton = null)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Key = key;
            MouseButton = mouseButton;
        }
    }
}
