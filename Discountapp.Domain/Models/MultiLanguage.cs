using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Discountapp.Infrastructure;
using Newtonsoft.Json;

namespace Discountapp.Domain.Models
{
    public class MultiLanguage: INameable
    {
        #region Constructors
        protected MultiLanguage() { }
        protected MultiLanguage(string name)
        {
            this.Name = name;
        }
        protected MultiLanguage(INameMultiLanguageJson json)
        {
            this.Name = json.Name;
            this.NameMultiLangJson = json.ToString();
        }
        #endregion

        [JsonIgnore]
        public virtual string NameMultiLangJson { get; set; }
        public virtual string Name { get; set; }

        #region NotMapped properties   
        [NotMapped]
        public NameMultiLanguageJson NameMultiLangJsonObject => this.MakeNameMultiLangJsonObject<NameMultiLanguageJson>(this.NameMultiLangJson, this.Name);
        [NotMapped]
        public string NameMultiLang
        {
            get { return NameMultiLangJsonObject.Value; }
            set
            {
                if(value == null) throw new ArgumentNullException(nameof(value));
            }
        }
        #endregion

        #region helpers

        public static string GenerateNameMultiLangJson(IEnumerable<INameMultiLanguageJson> list )
        {
            return JsonConvert.SerializeObject(list);
        }
        protected TNameMultiLanguageJson MakeNameMultiLangJsonObject<TNameMultiLanguageJson>(string nameMultiLangJson, string defaultName)
            where TNameMultiLanguageJson : INameMultiLanguageJson, new()
        {
            //Default result
            var result = new TNameMultiLanguageJson
            {
                Name = Culture.GetDefaultCulture(),
                Value = defaultName
            };

            //If property not null or empty
            if(!string.IsNullOrEmpty(nameMultiLangJson))
            {
                var currentLang = Culture.GetCurrentCulture();
                var langNameList = JsonConvert
                    .DeserializeObject
                    <List<TNameMultiLanguageJson>>(nameMultiLangJson);

                if((langNameList != null) &&
                    langNameList.Any(l => l.Name.Equals(currentLang, StringComparison.OrdinalIgnoreCase)))
                {
                    result = langNameList.SingleOrDefault
                        (
                            predicate: l => string.Equals(l.Name, currentLang, StringComparison.CurrentCultureIgnoreCase)
                        );

                    if(result != null && string.IsNullOrEmpty(result.Value))
                        result.Value = Name;
                }
            }


            return result;
        }
        #endregion
    }
}
