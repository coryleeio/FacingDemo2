using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Move : TargetableAction
    {
        public override int TimeCost
        {
            get
            {
                return 250;
            }
        }

        public override bool IsEndable
        {
            get
            {
                return isDoneInternal;
            }
        }

        public const float TimeBetweenTiles = 0.25f;

        private Vector2 LerpCurrentPosition;
        private Vector2 LerpTargetPosition;

        private float ElapsedMovementTime;

        public Point TargetPosition;
        private bool isDoneInternal = false;

        public override void Enter()
        {
            base.Enter();
            LerpCurrentPosition = MathUtil.MapToWorld(Source.Position);
            LerpTargetPosition = MathUtil.MapToWorld(TargetPosition);
            if(Source.IsPlayer)
            {
                Camera.main.GetComponent<GameSceneCameraDriver>().NewTarget(TargetPosition);
                Context.UIController.LootWindow.Hide();
            }
        }


        public override void Do()
        {
            base.Do();
            Assert.IsTrue(TargetPosition != null);
            ElapsedMovementTime += Time.deltaTime;
            if (ElapsedMovementTime > TimeBetweenTiles)
            {
                ElapsedMovementTime = TimeBetweenTiles;
            }

            var lerpPercentarge = ElapsedMovementTime / TimeBetweenTiles;
            var targetVectorPos = LerpTargetPosition;
            var _lerpPos = Vector2.Lerp(LerpCurrentPosition, targetVectorPos, lerpPercentarge);

            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = _lerpPos;
            }
            if (Vector2.Distance(_lerpPos, targetVectorPos) < 0.005f)
            {
                isDoneInternal = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            // Release old position
            if (Source.BlocksPathing)
            {
                Context.GameStateManager.Game.CurrentLevel.Grid[Source.Position].Walkable = true;
            }

            Context.EntitySystem.Deregister(Source, Context.GameStateManager.Game.CurrentLevel);

            // Move the view to the new position
            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = MathUtil.MapToWorld(TargetPosition);
            }
            
            // Actually set new position
            Source.Position = TargetPosition;

            // Lock new position
            if (Source.BlocksPathing)
            {
                Context.GameStateManager.Game.CurrentLevel.Grid[Source.Position].Walkable = false;
            }
            Context.EntitySystem.Register(Source, Context.GameStateManager.Game.CurrentLevel);

            foreach (var triggerThatMightGoOff in Context.GameStateManager.Game.CurrentLevel.Entitys)
            {
                if(triggerThatMightGoOff.Trigger != null && triggerThatMightGoOff.Trigger.Ability.TriggeredBy == TriggerType.OnTriggerStep)
                {
                    var points = MathUtil.GetPointsByOffset(triggerThatMightGoOff.Position, triggerThatMightGoOff.Trigger.Offsets);
                    CombatUtil.PerformTriggerStepAbilityIfSteppedOn(Source, triggerThatMightGoOff, points);
                }
            }

            if(Source.IsPlayer)
            {
                var level = Context.GameStateManager.Game.CurrentLevel;
                var entitiesInPos = level.Grid[Source.Position].EntitiesInPosition;
                var deadEntitiesInPos = entitiesInPos.FindAll((entInPos) => { return entInPos.Body != null && entInPos.Body.IsDead && entInPos.Inventory.HasAnyItems; });

                if (deadEntitiesInPos.Count > 0)
                {
                    Context.UIController.InputHint.ShowText("Press <color=yellow>F</color> to loot...");
                }
                else
                {
                    Context.UIController.InputHint.Hide();
                }
            }
        }

        public override bool IsValid()
        {
            return (!Source.BlocksPathing || (Source.BlocksPathing && Context.GameStateManager.Game.CurrentLevel.Grid[TargetPosition].Walkable));
        }
    }
}
