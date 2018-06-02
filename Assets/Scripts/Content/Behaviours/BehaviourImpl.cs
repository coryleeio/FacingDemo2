using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class BehaviourImpl : IHasApplicationContext
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

        [JsonIgnore]
        protected ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
        }
    }
}
