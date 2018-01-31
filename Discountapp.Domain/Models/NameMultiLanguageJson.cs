namespace Discountapp.Domain.Models
{
    public class NameMultiLanguageJson : INameMultiLanguageJson
    {
        public NameMultiLanguageJson() { }
        public NameMultiLanguageJson(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
