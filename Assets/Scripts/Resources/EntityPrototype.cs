using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public ProbabilityTable<string> NameOptions;
        public bool BlocksPathing { get; set; }
        public Body Body;
        public Trigger Trigger;
        public Motor Motor;
        public View ViewComponent;
        public Behaviour TurnComponent;

        // Helper for when you just want to set a single name and dont want a list of them
        public string Name
        {
            set
            {
                NameOptions = new ProbabilityTable<string>()
                {
                    Resolution = TableResolutionStrategy.OneOf,
                    Values = new List<ProbabilityTableTuple<string>>()
                    {
                        new ProbabilityTableTuple<string>()
                        {
                            NumberOfRolls = 1,
                            Value = value,
                            Weight = 100,
                        },
                    }
                };
            }
        }
    }
}
