namespace Kalavarda.Primitives.Units.Fight
{
    public class Fight
    {
        private readonly ICollection<UnitInfo> _members = new List<UnitInfo>();

        public IReadOnlyCollection<uint> MemberIds
        {
            get
            {
                lock (_members)
                    return _members.Select(m => m.Id).ToArray();
            }
        }

        public Fight(Unit member1, Unit member2)
        {
            Add(member1);
            Add(member2);
        }

        public void Add(Unit member)
        {
            lock (_members)
                if (_members.All(ui => ui.Id != member.Id))
                    _members.Add(new UnitInfo(member));
        }

        public class UnitInfo
        {
            public uint Id { get; }

            public string Name { get; }

            public UnitInfo(Unit unit)
            {
                Id = unit.Id;
                Name = unit.GetType().Name;
            }
        }
    }
}
