using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Gamepackage
{
    public class LootCorpse : Ability
    {
        public override string DisplayName
        {
            get
            {
                return "LootCorpse";
            }
        }

        public override string Description
        {
            get
            {
                return "LootCorpse";
            }
        }

        public override int TimeCost
        {
            get
            {
                return 0;
            }
        }

        [JsonIgnore]
        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        public override TriggerType TriggeredBy
        {
            get
            {
                return TriggerType.OnTriggerStep;
            }
        }


        [JsonIgnore]
        public TriggerStepContext AbilityTriggerContext;

        public override AbilityTriggerContext Perform(AbilityTriggerContext abilityTriggerContext)
        {
            AbilityTriggerContext = (TriggerStepContext)abilityTriggerContext;
            Debug.Log("Opening Loot UI...");
            return abilityTriggerContext;
        }

        public override bool CanPerform(AbilityTriggerContext abilityTriggerContext)
        {
            var ctx = (TriggerStepContext)abilityTriggerContext;
            foreach (var target in ctx.Targets)
            {
                if (target.IsPlayer)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
