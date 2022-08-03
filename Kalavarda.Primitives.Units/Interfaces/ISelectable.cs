namespace Kalavarda.Primitives.Units.Interfaces
{
    public interface ISelectableEvents
    {
        event Action<ISelectable> IsSelectableChanged;
    }

    public interface ISelectable : ISelectableEvents
    {
        public bool IsSelectable { get; }
    }
}
