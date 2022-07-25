using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units.Aggregators;

public interface ICreatureEventAggregator
{
    event Action<ICreature> Died;
}