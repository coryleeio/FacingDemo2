using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    // Weights are interpreted to be out of 100 in all cases
    public enum TableResolutionStrategy
    {
        OneOf, // Guarantee exactly one thing from the list roll weight/100 1 time for each item.  
               // The sum of the weights must == 100

        AnyOf, // Iterate each Tuple, roll weight/100 NumberOfRolls times and return 0-N results.
    }

    public class ProbabilityTable<TProb>
    {
        public List<ProbabilityTableTuple<TProb>> Values = new List<ProbabilityTableTuple<TProb>>(0);
        public TableResolutionStrategy Resolution;

        public TProb NextSingleItem()
        {
            var nextRet = Next();
            Assert.IsTrue(nextRet.Count == 1);
            return nextRet[0];
        }

        public List<TProb> Next()
        {
            if (Values.Count == 0)
            {
                return new List<TProb>();
            }

            var aggregate = new List<TProb>();
            if (Resolution == TableResolutionStrategy.AnyOf)
            {
                foreach (var tuple in Values)
                {
                    for (var numberOfRolls = 0; numberOfRolls < tuple.NumberOfRolls; numberOfRolls++)
                    {
                        if (Random.Range(0.0f, 1.0f) <= (tuple.Weight / 100))
                        {
                            aggregate.Add(tuple.Value);
                        }
                    }
                }
                return aggregate;
            }
            else if (Resolution == TableResolutionStrategy.OneOf)
            {
                var probabilitySum = 0.0f;
                var randomValue = Random.Range(0.0f, 1.0f);
                foreach (var tuple in Values)
                {
                    Assert.IsTrue(tuple.NumberOfRolls == 1);
                    probabilitySum += (tuple.Weight / 100);
                    if (randomValue <= probabilitySum)
                    {
                        aggregate.Add(tuple.Value);
                        Assert.IsTrue(aggregate.Count == 1);
                        return aggregate;
                    }
                }
                throw new InvariantBrokenException("Sum of probabilities must not have been correct.  " +
                    "Table should not be able to return 0 results after rolling all tuples." + 
                    "You should add logic to check as table build time that the sum of the tuples == 100");
            }
            else
            {
                throw new NotImplementedException("Not implemented");
            }
        }
    }

    public class ProbabilityTableTuple<TTuple>
    {
        public int NumberOfRolls = 1;
        public int Weight;
        public TTuple Value;
    }
}

