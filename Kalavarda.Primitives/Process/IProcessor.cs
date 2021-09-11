using System;
using System.Collections.Generic;

namespace Kalavarda.Primitives.Process
{
    public interface IProcessor
    {
        void Add(IProcess process, bool stopIncompatible = true);
        
        /// <summary>
        /// Выбирает все процессы указанного типа, подходящие под условие
        /// </summary>
        IEnumerable<T> Get<T>(Func<T, bool> whereClause = null) where T: IProcess;
    }
}