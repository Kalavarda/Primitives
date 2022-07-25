using System;

namespace Kalavarda.Primitives.WPF.Binds;

public interface IKeyBindsController
{
    event Action<KeyBind> BindActivated;
}