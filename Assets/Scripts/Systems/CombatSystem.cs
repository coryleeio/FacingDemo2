
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class CombatSystem
    {
        private List<Entity> DyingEntitys = new List<Entity>(0);
        private Color DeathColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

        public CombatSystem() {}

        public void Process()
        {
            foreach (var entity in DyingEntitys)
            {
                if (entity.View != null && entity.View.ViewGameObject != null)
                {
                    entity.Body.ElapsedTimeDead = entity.Body.ElapsedTimeDead += Time.deltaTime;
                    if (entity.Body.ElapsedTimeDead > 1.0f)
                    {
                        if (entity.View.ViewType == ViewType.StaticSprite)
                        {
                            var spriteRenderer = entity.View.ViewGameObject.GetComponent<SpriteRenderer>();
                            var firstPhasePercentage = (entity.Body.ElapsedTimeDead - 1.0f) / 1f;
                            var secondPhasePErcentage = (entity.Body.ElapsedTimeDead - 2f) / 1f;

                            if (firstPhasePercentage < 1.0f)
                            {
                                spriteRenderer.color = Color.Lerp(Color.white, Color.black, firstPhasePercentage);
                            }
                            else if (secondPhasePErcentage < 1.0f)
                            {
                                spriteRenderer.color = Color.Lerp(Color.black, DeathColor, secondPhasePErcentage);
                            }
                            else if (entity.View != null && entity.View.ViewGameObject != null)
                            {
                                GameObject.Destroy(entity.View.ViewGameObject);
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
                return ent.Body.ElapsedTimeDead > 9.0f;
            }
            );
        }

        public void TryToMoveToward(Entity entity, Point Position)
        {
            var moveToward = ServiceLocator.PrototypeFactory.BuildEntityAction<TryMoveToward>(entity);
            moveToward.TargetPoint = new Point(Position.X, Position.Y);
            entity.Behaviour.ActionList.AddLast(moveToward);
        }

        public void EndTurn(Entity entity)
        {
            var endTurn = ServiceLocator.PrototypeFactory.BuildEntityAction<EndTurn>(entity);
            entity.Behaviour.ActionList.AddLast(endTurn);
        }

        public void Wait(Entity entity)
        {
            var wait = ServiceLocator.PrototypeFactory.BuildEntityAction<Wait>(entity);
            entity.Behaviour.ActionList.AddLast(wait);
        }

        public void AttackInMelee(Entity entity, Entity player)
        {
            var attack = ServiceLocator.PrototypeFactory.BuildEntityAction<MeleeAttack>(entity);
            attack.TargetId = player.Id;
            entity.Behaviour.ActionList.AddLast(attack);
        }

        public bool CanMelee(Entity a, Entity b)
        {
            if(a == null || b == null)
            {
                return false;
            }
            return a.Position.IsAdjacentTo(b.Position) && a.Position.IsOrthogonalTo(b.Position);
        }

        public void DealDamage(Entity source, Entity target, int damage)
        {
            if (!target.IsCombatant)
            {
                throw new NotImplementedException("Cannot deal damage to non combatants");
            }
            if(target.Body.CurrentHealth <= 0)
            {
                // if you keep hitting him he doesn't get dead-er..
                return;
            }
            target.Body.CurrentHealth = target.Body.CurrentHealth - damage;
            var sourceMessage = source.Name;
            var targetMessage = target.Name;
            ServiceLocator.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("{0}", damage), target.IsPlayer ? Color.red : Color.magenta, 35, MathUtil.MapToWorld(target.Position));
            ServiceLocator.UIController.TextLog.AddText(string.Format("{0} hit {1} for {2} points of damage!", sourceMessage, targetMessage, damage));

            if (target.Body.CurrentHealth <= 0)
            {
                ServiceLocator.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("Dead!", damage), Color.black, 35, MathUtil.MapToWorld(target.Position));
                ServiceLocator.UIController.TextLog.AddText(string.Format("{0} has been slain!", targetMessage));
                target.Body.IsDead = true;
                var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
                if(!target.IsPlayer)
                {
                    ServiceLocator.EntitySystem.Deregister(target, level);
                }

                if (target.BlocksPathing)
                {
                    ServiceLocator.GameStateManager.Game.CurrentLevel.TilesetGrid[target.Position].Walkable = true;
                }
                DyingEntitys.Add(target);
            }
        }
    }
}
