using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gamepackage
{
    // A list of effects, the intent being to manage the lifecycle of effects because effects
    // are stored in numerous places throughout the state heirarchy, but need to conform
    // to the same lifecycle rules

    public class EffectList
    {
        [JsonProperty]
        private List<Effect> InternalList;

        public EffectList()
        {
            InternalList = new List<Effect>(0);
        }

        [JsonIgnore]
        public List<Effect> Values
        {
            get
            {
                return InternalList;
            }
        }

        public void Add(Entity owner, Effect input)
        {
            InternalList.Add(input);
            if(owner != null)
            {
                input.OnApply(owner);
            }
        }

        public void Remove(Entity owner, Effect input)
        {
            InternalList.Remove(input);
            if (owner != null)
            {
                input.OnRemove(owner);
            }
            if (Context.UIController != null && input.RemovalText != null && input.RemovalText != "")
            {
                Context.UIController.TextLog.AddText(input.RemovalText);
            }
        }

        [JsonIgnore]
        public int Count
        {
            get
            {
                return InternalList.Count;
            }
        }

        public void AddAll(Entity owner, List<Effect> input)
        {
            foreach(var effect in input)
            {
                Add(owner, effect);
            }
        }

        public void RemoveAll(Entity owner, Predicate<Effect> predicate)
        {
            var toRemove = InternalList.FindAll(predicate);
            foreach(var effect in toRemove)
            {
                Remove(owner, effect);
            }
        }

        public List<Effect> FindAll(Predicate<Effect> predicate)
        {
            return InternalList.FindAll(predicate);
        }
    }
}
