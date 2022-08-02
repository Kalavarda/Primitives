using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units.Interfaces;

public interface IFighter: IHasId, IHasName
{
    uint Id { get; }

    string Name { get; }
}