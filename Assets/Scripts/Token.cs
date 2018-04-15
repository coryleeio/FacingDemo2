
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamepackage
{
    public class Token : IHandleMessage
    {
        public Token()
        {

        }

        public int id;

        public List<string> Tags;

        public Motor Motor;

        public Inventory Inventory;

        public Equipment Equipment;

        public Behaviour Behaviour;

        public View View;

        public Persona Persona;

        public TriggerBehaviour TriggerBehaviour;

        public void HandleMessage(Message messageToHandle)
        {
            throw new NotImplementedException();
        }
    }
}