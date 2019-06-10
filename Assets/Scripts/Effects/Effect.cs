using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Effect
    {
        public Dictionary<string, string> Data;
        public Dictionary<Attributes, int> Attributes;

        public string TemplateIdentifier;
        [JsonIgnore]
        private EffectTemplate _template;
        [JsonIgnore]
        public EffectTemplate Template
        {
            get
            {
                if (_template == null)
                {
                    _template = Context.ResourceManager.Load<EffectTemplate>(TemplateIdentifier);
                }
                return _template;
            }
        }

        public string effectImplClassName;
        [JsonIgnore]
        private EffectImpl _effectImpl;
        [JsonIgnore]
        public EffectImpl EffectImpl
        {
            get
            {
                if(_effectImpl == null)
                {
                    _effectImpl = Context.ResourceManager.CreateInstanceFromAbstractOrInterfaceTypeAndName(typeof(EffectImpl), effectImplClassName) as EffectImpl;
                }
                return _effectImpl;
            }
        }

        public int TurnsRemaining;

        [JsonIgnore]
        public bool ShouldExpire
        {
            get
            {
                return !Template.HasUnlimitedDuration && TurnsRemaining <= 0;
            }
        }

        public string Name
        {
            get
            {
                return Template.LocalizationPrefix + ".name";
            }
        }

        public string Description
        {
            get
            {
                return Template.LocalizationPrefix + ".description";
            }
        }
        public string ItemDescription
        {
            get
            {
                return Template.LocalizationPrefix + ".item.description";
            }
        }
        public string ApplyMessage
        {
            get
            {
                return Template.LocalizationPrefix + ".apply";
            }
        }
        public string RemoveMessage
        {
            get
            {
                return Template.LocalizationPrefix + ".remove";
            }
        }
    }
}
