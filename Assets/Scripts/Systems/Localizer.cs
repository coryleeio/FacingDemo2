using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet;
using YamlDotNet.Serialization;

namespace Gamepackage
{
    public class Localizer
    {
        public enum LocalizerLanguages
        {
            english,
        }

        public LocalizerLanguages LanguageSetting;
        private LocalizerLanguages LoadedLanguage;
        private Dictionary<string, string> Lookup;

        public Localizer() { }

        public string LookupValueForLanguage(string input)
        {
            if(Lookup == null || LanguageSetting != LoadedLanguage)
            {
                Lookup = new Dictionary<string, string>();
                var textAsset = Resources.Load<TextAsset>("Localization/" + LanguageSetting);
                var deserializer = new Deserializer();
                Lookup = deserializer.Deserialize<Dictionary<string, string>>(textAsset.text);
                LoadedLanguage = LanguageSetting;
            }
            if(Lookup.ContainsKey(input))
            {
                return Lookup[input];
            }
            else
            {
                return input;
            }
            
        }
    }
}
