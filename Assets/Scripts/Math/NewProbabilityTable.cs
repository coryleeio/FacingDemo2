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

        public void Remove(NewProbabilityTableParcel entry)
        {
            Entries.Remove(entry);
            WeightTotal -= entry.Weight;
        }

        public List<string> Resolve()
        {
            var weightTotalTraversed = 0;
            var possibleReturns = new List<string>();
            var roll = Random.Range(0, WeightTotal);
            foreach (var entry in Entries)
            {
                weightTotalTraversed += entry.Weight;
                if(roll <= weightTotalTraversed)
                {
                    possibleReturns.AddRange(entry.Values);
                    break;
                }
            }
            var returnVals = new List<string>();
            foreach(var possibleReturn in possibleReturns)
            {
                if(possibleReturn.StartsWith("TABLE_"))
                {
                    // Support recursive tables.
                    var returnTable = Context.ResourceManager.Load<NewProbabilityTable>(possibleReturn);
                    returnVals.AddRange(returnTable.Resolve());
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
