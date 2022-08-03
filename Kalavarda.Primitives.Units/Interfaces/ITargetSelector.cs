namespace Kalavarda.Primitives.Units.Interfaces;

public interface ITargetSelector
{
    ISelectable Select(bool fightersOnly = false);
}