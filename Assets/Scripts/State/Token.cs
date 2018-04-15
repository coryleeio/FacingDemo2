
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamepackage
{
    public class Token : IHandleMessage, IWasBuiltFromAPrototype, IHaveAnId
    {
        public Token()
        {

        }

        public int Id { get; set; }

        public List<string> Tags = new List<string>();

        public Motor Motor;

        public Inventory Inventory;

        public Equipment Equipment;

        public Behaviour Behaviour;

        public View View;

        public Persona Persona;

        public TriggerBehaviour TriggerBehaviour;

        public string PrototypeUniqueIdentifier { get; set; }

        public void HandleMessage(Message messageToHandle)
        {
            throw new NotImplementedException();
        }
    }
}