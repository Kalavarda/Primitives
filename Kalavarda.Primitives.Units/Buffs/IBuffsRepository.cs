namespace Kalavarda.Primitives.Units.Buffs
{
    public interface IReadonlyBuffsRepository
    {
        BuffType GetById(uint id);
    }
}
