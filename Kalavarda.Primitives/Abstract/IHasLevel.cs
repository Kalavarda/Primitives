using System;

namespace Kalavarda.Primitives.Abstract
{
    public interface IHasLevel
    {
        ushort Level { get; }

        event Action<IHasLevel> LevelChanged;
    }
}
