using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Move : Action
    {
        [JsonIgnore]
        public Entity Source;

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
            var newDirection = MathUtil.RelativeDirection(Source.Position, TargetPosition);


            var sortable = Source.Sortable;
            if (sortable != null && TargetPosition != null && newDirection == Direction.SouthEast || newDirection == Direction.SouthWest || newDirection == Direction.East || newDirection == Direction.South)
            {
                // Sort these from the higher index position immediately before moving them so they are sorted
                // corectly. Other directions should keep the higher index until the move is complete.
                sortable.Position = TargetPosition;
            }


            Source.Direction = newDirection;

            if (Source.View != null && Source.View.SkeletonAnimation != null)
            {
                var skeletonAnimation = Source.View.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Walk, newDirection), true);
            }

            if (Source.IsPlayer)
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
            Context.EntitySystem.Deregister(Source, Context.GameStateManager.Game.CurrentLevel);

            // Move the view to the new position
            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = MathUtil.MapToWorld(TargetPosition);
            }
            
            // Actually set new position
            Source.Position = TargetPosition;

            var sortable = Source.Sortable;
            if (sortable != null)
            {
                sortable.Position = TargetPosition;
            }

            // Lock new position
            Context.EntitySystem.Register(Source, Context.GameStateManager.Game.CurrentLevel);

            foreach (var triggerThatMightGoOff in Context.GameStateManager.Game.CurrentLevel.Entitys)
            {
                var onStepTriggers = triggerThatMightGoOff.GetEffects((effectInQuestion) => { return effectInQuestion.CanTriggerOnStep(); });
                foreach (var onStepTrigger in onStepTriggers)
                {
                    var points = MathUtil.GetPointsByOffset(triggerThatMightGoOff.Position, triggerThatMightGoOff.Trigger.Offsets);
                    CombatUtil.PerformTriggerStepAbilityIfSteppedOn(Source, triggerThatMightGoOff, points);
                }
            }

            if(Source.IsPlayer)
            {
                var level = Context.GameStateManager.Game.CurrentLevel;
                var entitiesInPos = level.Grid[Source.Position].EntitiesInPosition;
                HandleInputHints(entitiesInPos);
            }
        }

        public static void HandleInputHints(List<Entity> entitiesInPos)
        {
            var lootableEntitiesInPosition = entitiesInPos.FindAll(CombatUtil.LootableEntities);
            var trigggerEntitiesInPos = entitiesInPos.FindAll((entInPos) => { return entInPos.Trigger != null; });
            Effect triggerOnPressEffect = null;

            foreach (var triggerEnt in trigggerEntitiesInPos)
            {
                foreach (var effect in triggerEnt.Trigger.Effects)
                {
                    if (effect.CanTriggerOnPress())
                    {
                        triggerOnPressEffect = effect;
                    }
                }
            }

            if (triggerOnPressEffect != null)
            {
                Context.UIController.InputHint.ShowText(triggerOnPressEffect.Description);
            }

            else if (lootableEntitiesInPosition.Count > 0)
            {
                Context.UIController.InputHint.ShowText("show.loot.message".Localize());
            }
            else
            {
                Context.UIController.InputHint.Hide();
            }
        }

        public override bool IsValid()
        {
            return (!Source.BlocksPathing || (Source.BlocksPathing && Context.GameStateManager.Game.CurrentLevel.Grid[TargetPosition].Walkable && Context.GameStateManager.Game.CurrentLevel.Grid[TargetPosition].EntitiesInPosition.FindAll(CombatUtil.HittableEntities).Count == 0));
        }
    }
}
