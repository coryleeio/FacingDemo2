using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public enum TableResolutionStrategy
    {
        Options, // Guarantee exactly one thing from the list
        Chance, // Iterate each Tuple, roll NumberOfRolls times based on it's proportion of the total weight in the table, return 0-N results.
    }

    
    public class ProbabilityTable<TProb>
    {
        public List<ProbabilityTableTuple<TProb>> Values;
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
            var totalWeight = 0;
            foreach (var tuple in Values)
            {
                totalWeight += tuple.Weight;
            }

            var aggregate = new List<TProb>();
            if (Resolution == TableResolutionStrategy.Chance)
            {
                foreach (var tuple in Values)
                {
                    for (var numberOfRolls = 0; numberOfRolls < tuple.NumberOfRolls; numberOfRolls++)
                    {
                        if (Random.Range(0.0f, 1.0f) <= (tuple.Weight / totalWeight))
                        {
                            aggregate.Add(tuple.Value);
                        }
                    }
                }
                return aggregate;
            }
            else if (Resolution == TableResolutionStrategy.Options)
            {
                var probabilitySum = 0.0f;
                var randomValue = Random.Range(0.0f, 1.0f);
                foreach (var tuple in Values)
                {
                    Assert.IsTrue(tuple.NumberOfRolls == 1);
                    probabilitySum += (tuple.Weight / totalWeight);
                    if (randomValue <= probabilitySum)
                    {
                        aggregate.Add(tuple.Value);
                        Assert.IsTrue(aggregate.Count == 1);
                        return aggregate;
                    }
                }

                Debug.LogWarning("Choice table defaulted to returning a random object");
                // Floating point rounding might cause us to have returned nothing here.  
                // In that case just return a random thing.
                aggregate.Add(MathUtil.ChooseRandomElement<ProbabilityTableTuple<TProb>>(Values).Value);
                return aggregate;
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

