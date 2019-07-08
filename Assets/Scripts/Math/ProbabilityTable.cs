using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class ProbabilityTable
    {
        private List<ProbabilityTableParcel> Entries = new List<ProbabilityTableParcel>();
        private int WeightTotal = 0;

        public void Add(ProbabilityTableParcel entry)
        {
            Entries.Add(entry);
            WeightTotal += entry.Weight;
        }

        public void AddRange(List<ProbabilityTableParcel> entries)
        {
            foreach (var entry in entries)
            {
                Add(entry);
            }
        }

        public void Remove(ProbabilityTableParcel entry)
        {
            Entries.Remove(entry);
            WeightTotal -= entry.Weight;
        }

        public string RollAndChooseOne()
        {
            var outputOfRoll = Roll();
            Assert.IsTrue(outputOfRoll.Count > 0);
            return MathUtil.ChooseRandomElement<string>(outputOfRoll);
        }

        public List<string> Roll()
        {
            var weightTotalTraversed = 0;
            var possibleReturns = new List<string>();
            var roll = Random.Range(0, WeightTotal + 1); // +1 is to make it inclusive of the weight total value
            foreach (var entry in Entries)
            {
                weightTotalTraversed += entry.Weight;
                if (roll <= weightTotalTraversed)
                {
                    possibleReturns.AddRange(entry.Values);
                    break;
                }
            }
            var returnVals = new List<string>();
            foreach (var possibleReturn in possibleReturns)
            {
                if (Context.ResourceManager.Contains<ProbabilityTable>(possibleReturn))
                {
                    var returnTable = Context.ResourceManager.Load<ProbabilityTable>(possibleReturn);
                    returnVals.AddRange(returnTable.Roll());
                }
                else
                {
                    returnVals.Add(possibleReturn);
                }

            }
            return returnVals;
        }
    }
}
