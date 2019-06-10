using System.Collections.Generic;

namespace Gamepackage
{
    public static class TriggerFactory
    {
        public static Trigger Build(string templateIdentifier)
        {
            var template = Context.ResourceManager.Load<TriggerTemplate>(templateIdentifier);
            var state = new Trigger();
            state.TemplateIdentifier = templateIdentifier;
            state.Data = new Dictionary<string, string>();
            return state;
        }
    }
}
