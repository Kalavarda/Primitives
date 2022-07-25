using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units.EventAggregators;

public interface ICreatureEventAggregator
{
    event Action<ICreature> Died;
}