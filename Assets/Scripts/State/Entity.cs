
using Newtonsoft.Json;
using System;

namespace Gamepackage
{
    [Serializable]
    public class Entity
    {
        public Entity() { }

        public int Id { get; set; }

        public string Name;

        public Point Position;

        public Body Body;

        public Trigger Trigger;

        public View View;

        public Behaviour Behaviour;

        public Inventory Inventory;

        public UniqueIdentifier PrototypeIdentifier { get; set; }

        public bool BlocksPathing = false;

        [JsonIgnore]
        public bool IsPlayer
        {
            get
            {
                return Behaviour != null && Behaviour.IsPlayer;
            }
        }

        [JsonIgnore]
        public bool IsNPC
        {
            get
            {
                return Behaviour != null && !Behaviour.IsPlayer;
            }
        }

        [JsonIgnore]
        public bool IsCombatant
        {
            get
            {
                return Body != null;
            }
        }

        public void Rewire()
        {
            if(Trigger != null)
            {
                Trigger.Rewire(this);
            }
            if(Body != null)
            {
                Body.Rewire(this);
            }
            if(View != null)
            {
                View.Rewire(this);
            }
            if(Behaviour != null)
            {
                Behaviour.Rewire(this);
            }
        }
    }
}