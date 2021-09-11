using System;
using System.Collections.Generic;

namespace Kalavarda.Primitives.Process
{
    public interface IProcess
    {
        event Action<IProcess> Completed;
        
        void Process(TimeSpan delta);

        void Stop();
    }

    public interface IIncompatibleProcess
    {
        /// <summary>
        /// Определяет процессы, которые должны быть отменены с добавлением этого процесса
        /// </summary>
        IReadOnlyCollection<IProcess> GetIncompatibleProcesses(IReadOnlyCollection<IProcess> processes);
    }
}
