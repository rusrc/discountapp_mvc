using System.ComponentModel.DataAnnotations.Schema;
using Discountapp.Domain.Models;

namespace Discountapp.Domain
{
    public class CategoryMultiLanguage : MultiLanguage
    {
        public string CategoryName { get; set; }
        public string CategoryNameMultiLangJson { get; set; }

        [NotMapped]
        public override string Name { get; set; }
        [NotMapped]
        public override string NameMultiLangJson { get; set; }

        public NameMultiLanguageJson CategoryNameMultiLangJsonObject => MakeNameMultiLangJsonObject<NameMultiLanguageJson>(CategoryNameMultiLangJson, CategoryName);
    }
}
