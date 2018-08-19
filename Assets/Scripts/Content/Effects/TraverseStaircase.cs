using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Gamepackage
{
    public class TraverseStaircase : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "Change level";
            }
        }

        public override string Description
        {
            get
            {
                return "Moves you to another level";
            }
        }

        [JsonIgnore]
        public AttackContext AbilityTriggerContext;

        public enum Params
        {
            TARGET_POSX,
            TARGET_POSY,
            TARGET_LEVEL_ID,
        }

        public override EffectTriggerType EffectApplicationTrigger
        {
            get
            {
                return EffectTriggerType.OnStep;
            }
        }

        public override AttackContext Apply(AttackContext abilityTriggerContext)
        {
            AbilityTriggerContext = abilityTriggerContext;
            var Parameters = AbilityTriggerContext.Source.Trigger.TriggerParameters;
            var levelId = Convert.ToInt32(Parameters[Params.TARGET_LEVEL_ID.ToString()]);
            var posX = Convert.ToInt32(Parameters[Params.TARGET_POSX.ToString()]);
            var posY = Convert.ToInt32(Parameters[Params.TARGET_POSY.ToString()]);
            var targetIncludesPlayer = false;
            foreach (var target in AbilityTriggerContext.Targets)
            {
                if (target.IsPlayer)
                {
                    targetIncludesPlayer = true;
                }
            }
            if (targetIncludesPlayer)
            {
                var oldLevel = Context.GameStateManager.Game.CurrentLevel;
                foreach (var entityInLevel in oldLevel.Entitys)
                {
                    if (entityInLevel.Behaviour != null && entityInLevel.Behaviour.Team == Team.PLAYER && !entityInLevel.Behaviour.IsPlayer)
                    {
                        // Add player followers to list of targets
                        AbilityTriggerContext.Targets.Add(entityInLevel);
                    }
                }
                Context.GameStateManager.Game.CurrentLevel.UnindexAll();
            }

            foreach (var target in AbilityTriggerContext.Targets)
            {
                target.Behaviour.NextAction = null;
                Context.FlowSystem.Steps.First.Value.Actions.Clear();
                var oldLevel = Context.GameStateManager.Game.CurrentLevel;
                if (target.BlocksPathing)
                {
                    oldLevel.Grid[target.Position].Walkable = true;
                }
                if (target.View.ViewGameObject != null)
                {
                    GameObject.Destroy(target.View.ViewGameObject);
                }
                var newLevel = Context.GameStateManager.Game.Dungeon.Levels[levelId];
                var pos = new Point(posX, posY);
                oldLevel.Entitys.Remove(target);
                target.Position = pos;
                newLevel.Entitys.Add(target);
            }
            if (targetIncludesPlayer)
            {
                Context.PlayerController.ActionList.Clear();
                Context.GameStateManager.Game.CurrentLevelIndex = levelId;
                if (levelId > Context.GameStateManager.Game.FurthestLevelReached)
                {
                    Context.GameStateManager.Game.FurthestLevelReached = levelId;
                }
                Context.Application.StateMachine.ChangeState(ApplicationStateMachine.GamePlayState);
            }
            return abilityTriggerContext;
        }

        public override bool CanApply(AttackContext ctx)
        {
            foreach (var target in ctx.Targets)
            {
                if (target.IsPlayer)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
