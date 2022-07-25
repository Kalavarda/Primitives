namespace Kalavarda.Primitives.WPF.Binds
{
    public class SkillBind
    {
        public SkillBind(string bindCode, string skillCode)
        {
            BindCode = bindCode;
            SkillCode = skillCode;
        }

        public string BindCode { get; }

        public string SkillCode { get; }
    }
}
