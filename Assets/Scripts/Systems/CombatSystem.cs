
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class CombatSystem
    {
        public ApplicationContext Context { get; set; }
        private List<Entity> DyingEntitys = new List<Entity>(0);
        private Color DeathColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

        public CombatSystem()
        {

        }

        public void Process()
        {
            foreach (var entity in DyingEntitys)
            {
                if (entity.View != null)
                {
                    entity.CombatantComponent.ElapsedTimeDead = entity.CombatantComponent.ElapsedTimeDead += Time.deltaTime;
                    if (entity.CombatantComponent.ElapsedTimeDead > 1.0f)
                    {
                        if (entity.ViewType == ViewType.StaticSprite)
                        {
                            var spriteRenderer = entity.View.GetComponent<SpriteRenderer>();
                            var firstPhasePercentage = (entity.CombatantComponent.ElapsedTimeDead - 1.0f) / 1f;
                            var secondPhasePErcentage = (entity.CombatantComponent.ElapsedTimeDead - 2f) / 1f;

                            if (firstPhasePercentage < 1.0f)
                            {
                                spriteRenderer.color = Color.Lerp(Color.white, Color.black, firstPhasePercentage);
                            }
                            else if (secondPhasePErcentage < 1.0f)
                            {
                                spriteRenderer.color = Color.Lerp(Color.black, DeathColor, secondPhasePErcentage);
                            }
                            else if (entity.View != null)
                            {
                                GameObject.Destroy(entity.View);
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }

            var removed = DyingEntitys.RemoveAll((ent) =>
            {
                return ent.CombatantComponent.ElapsedTimeDead > 9.0f;
            }
            );
        }

        public void DealDamage(Entity source, Entity target, int damage)
        {
            if (!target.IsCombatant)
            {
                return;
            }
            target.CombatantComponent.CurrentHealth = target.CombatantComponent.CurrentHealth - damage;
            if (target.CombatantComponent.CurrentHealth <= 0)
            {
                var level = Context.GameStateManager.Game.CurrentLevel;
                Context.EntitySystem.Deregister(target, level);
                if (target.EntityPrototype.BlocksPathing)
                {
                    Context.GameStateManager.Game.CurrentLevel.TilesetGrid[target.Position].Walkable = true;
                }
                DyingEntitys.Add(target);
            }
        }
    }
}
