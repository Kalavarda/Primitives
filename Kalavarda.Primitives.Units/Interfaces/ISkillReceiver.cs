namespace Kalavarda.Primitives.Units.Interfaces
{
    public interface ISkillReceiver
    {
        event Action<Unit, Unit> NegativeSkillReceived;
    }
}
