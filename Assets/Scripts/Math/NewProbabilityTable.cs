using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class NewProbabilityTable
    {
        private List<NewProbabilityTableParcel> Entries = new List<NewProbabilityTableParcel>();
        private int WeightTotal = 0;

        public void Add(NewProbabilityTableParcel entry)
        {
            Entries.Add(entry);
            WeightTotal += entry.Weight;
        }

        public void AddRange(List<NewProbabilityTableParcel> entries)
        {
            foreach (var entry in entries)
            {
                Add(entry);
            }
        }

        public void Remove(NewProbabilityTableParcel entry)
        {
            Entries.Remove(entry);
            WeightTotal -= entry.Weight;
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
                if (Context.ResourceManager.Contains<NewProbabilityTable>(possibleReturn))
                {
                    var returnTable = Context.ResourceManager.Load<NewProbabilityTable>(possibleReturn);
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
