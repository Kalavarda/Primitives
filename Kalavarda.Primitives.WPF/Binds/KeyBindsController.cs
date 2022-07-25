using System;
using System.Windows;

namespace Kalavarda.Primitives.WPF.Binds
{
    public class KeyBindsController: IDisposable, IKeyBindsController
    {
        private readonly IInputElement _inputElement;
        private readonly IKeyBinds _keyBinds;

        public KeyBindsController(IInputElement inputElement, IKeyBinds keyBinds)
        {
            _inputElement = inputElement ?? throw new ArgumentNullException(nameof(inputElement));
            _keyBinds = keyBinds ?? throw new ArgumentNullException(nameof(keyBinds));

            _inputElement.KeyDown += InputElement_KeyDown;
        }

        private void InputElement_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Handled)
                return;

            foreach (var bind in _keyBinds.Binds)
                if (bind.Key == e.Key)
                {
                    e.Handled = true;
                    BindActivated?.Invoke(bind);
                    break;
                }
        }

        public event Action<KeyBind> BindActivated;

        public void Dispose()
        {
            _inputElement.KeyDown -= InputElement_KeyDown;
        }
    }
}
