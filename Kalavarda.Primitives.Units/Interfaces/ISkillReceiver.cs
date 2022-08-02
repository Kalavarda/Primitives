namespace Kalavarda.Primitives.Units.Interfaces
{
    public interface ISkillReceiver
    {
        event Action<IFighter, IFighter> NegativeSkillReceived;
    }
}
