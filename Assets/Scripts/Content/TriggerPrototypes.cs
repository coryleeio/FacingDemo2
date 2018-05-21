using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public static class TriggerPrototypes
    {
        public static List<TriggerPrototype> LoadAll(TinyIoCContainer container)
        {
            var output = new List<TriggerPrototype>();

            var poisonDartTriggerAction = container.Resolve<PoisonDart>();
            // add params to trigger action here if needed
            var weakPoisonDart = new TriggerPrototype()
            {
                UniqueIdentifier = UniqueIdentifier.TRIGGER_WEAK_POISON_DART,
                TriggerAction = poisonDartTriggerAction,
            };
            output.Add(weakPoisonDart);

            return output;
        }
    }
}
