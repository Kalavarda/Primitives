using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Kalavarda.Primitives.Process
{
    public class MultiProcessor: IProcessor
    {
        private readonly Processor[] _processors;
        private byte _nextProcess;

        public MultiProcessor(int maxFrequency, CancellationToken cancellationToken)
        {
            _processors = new Processor[Environment.ProcessorCount];
            for (var i = 0; i < _processors.Length; i++)
                _processors[i] = new Processor(maxFrequency, cancellationToken);
        }

        public void Add(IProcess process, bool stopIncompatible = true)
        {
            if (stopIncompatible)
                if (process is IIncompatibleProcess incompatibleProcess)
                    foreach (var processor in _processors)
                        processor.StopIncompatible(incompatibleProcess);

            _processors[_nextProcess].Add(process, false);

            _nextProcess++;
            if (_nextProcess == _processors.Length)
                _nextProcess = 0;
        }

        public IEnumerable<T> Get<T>(Func<T, bool> whereClause = null) where T : IProcess
        {
            var result = Enumerable.Empty<T>();
            foreach (var p in _processors)
                result = result.Union(p.Get(whereClause));
            return result;
        }
    }
}
