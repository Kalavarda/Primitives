using Kalavarda.Primitives.Abstract;

namespace Kalavarda.Primitives.Units
{
    public class UnitChanges
    {
        public UnitChanges(float hp, IHasName skillOrItemName)
        {
            HP = hp;
            SkillOrItemName = skillOrItemName ?? throw new ArgumentNullException(nameof(skillOrItemName));
        }

        public float HP { get; set; }

        public IHasName SkillOrItemName { get; }
    }
}
