using System.Collections.Generic;

namespace Gamepackage
{
    public static class TriggerActionFactory
    {
        public static ITriggerableActionImpl Build(string FullName)
        {
            return Context.ResourceManager.CreateInstanceFromAbstractOrInterfaceTypeAndName(typeof(ITriggerableActionImpl), FullName) as ITriggerableActionImpl;
        }

        public static List<ITriggerableActionImpl> BuildAll(List<string> FullNames)
        {
            var aggregate = new List<ITriggerableActionImpl>();

            foreach (var fullName in FullNames)
            {
                aggregate.Add(Build(fullName));
            }
            return aggregate;
        }
    }
}
