using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Step
    {
        public LinkedList<Action> Actions = new LinkedList<Action>();

        public bool Done
        {
            get
            {
                return Actions.Count == 0;
            }
        }

        public void Do()
        {

            var completed = new List<Action>(0);
            try
            {
                foreach (var action in Actions)
                {
                    if (!action.Done)
                    {
                        action.Do();
                    }
                    else
                    {
                        completed.Add(action);
                    }
                }
            }
            catch(InvalidOperationException ex)
            {
                // List will be modified, by the actions themselves sometimes, if this happens just restart the loop.
                Do();
            }
            foreach(var action in completed)
            {
                action.Reset();
                Actions.Remove(action);
            }
        }
    }
}
