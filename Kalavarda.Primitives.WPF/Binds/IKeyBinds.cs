using System.Collections.Generic;

namespace Kalavarda.Primitives.WPF.Binds;

public interface IKeyBinds
{
    IReadOnlyCollection<KeyBind> Binds { get; }
}