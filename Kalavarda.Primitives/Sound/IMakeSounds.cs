using System;

namespace Kalavarda.Primitives.Sound
{
    public interface IMakeSounds
    {
        /// <summary>
        /// Нужно проиграть звук с указанным кодом
        /// </summary>
        event Action<string> PlaySound;
    }
}
