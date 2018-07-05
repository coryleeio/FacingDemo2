using System.Collections.Generic;

namespace Gamepackage
{


    // These exist so that you dont accidentally serialize parameters to an ability
    // or need to particularly worry about resetting them, 
    // since you will pass in a new context for each one you perform the ability
    // you dont have to reset it, clear its targets, remember not to serialize targets
    // or sources, etc.

    // The general convention is decide if the ability is the right type
    // if it is, build up a context
    // see if the ability canPerform() with the context specified(sources, targets, anything really)
    // if it can, call perform() with the same context.
    // Perform() should set the context to a local variable, and Start(), Do(), Exit()
    // etc should use it.

    public abstract class AbilityTriggerContext
    {

    }




}
