using System;

namespace Kalavarda.Primitives.Abstract;

public interface IHasDispose : IDisposable
{
    event Action<IHasDispose> Disposing;
}