using System.Collections.Generic;

namespace Kalavarda.Primitives.WPF.Binds
{
    public abstract class KeyBindsBase : IKeyBinds
    {
        public abstract IReadOnlyCollection<KeyBind> Binds { get; }
    }
}
