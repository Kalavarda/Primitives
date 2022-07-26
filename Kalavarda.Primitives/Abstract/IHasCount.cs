using System;

namespace Kalavarda.Primitives.Abstract
{
    public interface IHasCount
    {
        uint Count { get; }

        event Action<IHasCount, uint, uint> CountChanged;
    }
}
