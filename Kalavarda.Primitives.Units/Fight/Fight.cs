using Kalavarda.Primitives.Units.Interfaces;

namespace Kalavarda.Primitives.Units.Fight
{
    public class Fight
    {
        private readonly ICollection<UnitPhantom> _members = new List<UnitPhantom>();

        public IReadOnlyCollection<uint> MemberIds
        {
            get
            {
                lock (_members)
                    return _members.Select(m => m.Id).ToArray();
            }
        }

        public Fight(IFighter member1, IFighter member2)
        {
            Add(member1);
            Add(member2);
        }

        public void Add(IFighter member)
        {
            lock (_members)
                if (_members.All(ui => ui.Id != member.Id))
                {
                    // TODO: оставить только IFighter

                    if (member is UnitPhantom phantom)
                        _members.Add(phantom);
                    else if (member is Unit unit)
                        _members.Add(new UnitPhantom(unit));
                    else
                        throw new NotImplementedException();
                }
        }
    }
}
