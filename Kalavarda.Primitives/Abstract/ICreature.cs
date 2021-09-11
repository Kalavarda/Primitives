using System;
using Kalavarda.Primitives.Skills;

namespace Kalavarda.Primitives.Abstract
{
    public interface ICreature
    {
        RangeF HP { get; }

        bool IsAlive { get; }

        bool IsDead { get; }

        event Action<ICreature> Died;
    }

    public interface ICreatureExt: ICreature
    {
        void ChangeHP(float value, ISkilled initializer, ISkill skill);

        event Action<HpChange> HpChanged;
    }

    public class HpChange
    {
        public ISkilled Initializer { get; }

        public ISkill Skill { get; }

        public ICreature Creature { get; }

        public float Diff { get; }

        public HpChange(ICreature creature, float diff, ISkilled initializer, ISkill skill)
        {
            Initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            Skill = skill ?? throw new ArgumentNullException(nameof(skill));
            Creature = creature ?? throw new ArgumentNullException(nameof(creature));
            Diff = diff;
        }
    }
}
