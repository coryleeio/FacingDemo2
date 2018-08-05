using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Gamepackage
{
    public class TraverseStaircase : Ability
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
        public AbilityContext AbilityTriggerContext;

        public enum Params
        {
            TARGET_POSX,
            TARGET_POSY,
            TARGET_LEVEL_ID,
        }

        public override int TimeCost
        {
            get
            {
                return 0;
            }
        }

        [JsonIgnore]
        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        public override TriggerType TriggeredBy
        {
            get
            {
                return TriggerType.OnStep;
            }
        }

        public override void Exit()
        {
            base.Exit();
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
            if(targetIncludesPlayer)
            {
                var oldLevel = Context.GameStateManager.Game.CurrentLevel;
                foreach(var entityInLevel in oldLevel.Entitys)
                {
                    if(entityInLevel.Behaviour != null && entityInLevel.Behaviour.Team == Team.PLAYER && !entityInLevel.Behaviour.IsPlayer)
                    {
                        // Add player followers to list of targets
                        AbilityTriggerContext.Targets.Add(entityInLevel);
                    }
                }
                Context.GameStateManager.Game.CurrentLevel.UnindexAll();
            }

            foreach (var target in AbilityTriggerContext.Targets)
            {
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
        }

        public override AbilityContext Perform(AbilityContext abilityTriggerContext)
        {
            var step = new Step();
            AbilityTriggerContext = abilityTriggerContext; // Save this off for enter / exit
            step.Actions.AddFirst(this);
            Context.FlowSystem.Steps.AddAfter(Context.FlowSystem.Steps.First, step);
            return abilityTriggerContext;
        }

        public override bool CanPerform(AbilityContext ctx)
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
