namespace Kalavarda.Primitives.Units.Buffs
{
    public interface IReadonlyHasBuffs
    {
        IReadOnlyCollection<Buff> Buffs { get; }

        event Action<IReadonlyHasBuffs, Buff> BuffAdded;

        event Action<IReadonlyHasBuffs, Buff> BuffRemoved;
    }

    public interface IHasBuffs: IReadonlyHasBuffs
    {
        public void Add(Buff buff);

        public void Remove(Buff buff);
    }
}
