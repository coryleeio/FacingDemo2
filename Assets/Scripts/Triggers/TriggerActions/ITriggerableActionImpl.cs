using System.Collections.Generic;

namespace Gamepackage
{
    // A bit of code that can run to handle a trigger
    // typically used for press to interact kind of situations.
    public interface ITriggerableActionImpl
    {
        bool CanPerform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data);
        void Perform(Entity TriggerThatWentOff, Entity CauseOfTrigger, Dictionary<string, string> Data);

        string ActionNameLocalizationKey
        {
            get;
        }
    }
}
