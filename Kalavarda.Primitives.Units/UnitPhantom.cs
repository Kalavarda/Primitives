using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units;

/// <summary>
/// Копия юнита на случа его (юнита) смерти
/// </summary>
public class UnitPhantom : IFighter
{
    public uint Id { get; }

    public string Name { get; }
    
    public UnitPhantom(Unit unit)
    {
        Id = unit.Id;
        Name = unit.GetType().Name;
    }
}