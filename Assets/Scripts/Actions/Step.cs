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
            foreach(var action in completed)
            {
                action.Reset();
                Actions.Remove(action);
            }
        }
    }
}
