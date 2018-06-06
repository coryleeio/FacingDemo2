using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class BehaviourImpl
    {
        public void GetActionsForTurn()
        {
            SetActionsForThisTurn();
            Context.CombatSystem.EndTurn(Entity);
        }

        protected abstract void SetActionsForThisTurn();

        [JsonIgnore]
        public Entity Entity;

        public virtual bool IsPlayer
        {
            get
            {
                return false;
            }
        }

        public void InjectContext(Entity entity)
        {
            Entity = entity;
        }

    }
}
