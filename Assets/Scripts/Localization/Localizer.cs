using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;

namespace Gamepackage
{
    public class Localizer
    {
        public enum LocalizerLanguages
        {
            NotSet,
            English,
        }

        public LocalizerLanguages LanguageSetting;
        private LocalizerLanguages LoadedLanguage;
        private Dictionary<LocalizerLanguages, Dictionary<string, string>> Lookup;

        public Localizer() { }

        public string LookupValueForLanguage(string input)
        {
            if(LanguageSetting == LocalizerLanguages.NotSet)
            {
                LanguageSetting = LocalizerLanguages.English;
            }

            if(Lookup == null || !Lookup.ContainsKey(LanguageSetting) || LanguageSetting == LocalizerLanguages.NotSet)
            {
                return input;
            }
            if (Lookup[LanguageSetting].ContainsKey(input))
            {
                return Lookup[LanguageSetting][input];
            }
            else
            {
                return input;
            }
        }

        public void LoadDictForLanguage(LocalizerLanguages language, Dictionary<string, string> translations)
        {
            if (Lookup == null)
            {
                Lookup = new Dictionary<LocalizerLanguages, Dictionary<string, string>>();
            }
            if (!Lookup.ContainsKey(language))
            {
                Lookup[language] = new Dictionary<string, string>();
            }

            foreach (var pair in translations)
            {
                if (Lookup[language].ContainsKey(pair.Key))
                {
                    Debug.Log("Overwrote translation for: " + pair.Key);
                }
                Lookup[language][pair.Key] = pair.Value;
            }
        }
    }
}
