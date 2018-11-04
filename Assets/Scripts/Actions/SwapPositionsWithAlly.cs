using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class SwapPositionsWithAlly : Action
    {
        [JsonIgnore]
        public Entity Source;

        [JsonIgnore]
        public List<Entity> Targets = new List<Entity>(0);

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

        private Vector2 LerpCurrentPositionForSource;
        private Vector2 LerpTargetPositionForSource;

        private Vector2 LerpCurrentPositionForTarget;
        private Vector2 LerpTargetPositionForTarget;

        private float ElapsedMovementTime;

        private bool isDoneInternal = false;

        public override void Enter()
        {
            base.Enter();
            Assert.IsTrue(Targets.Count == 1);
            LerpCurrentPositionForSource = MathUtil.MapToWorld(Source.Position);
            LerpTargetPositionForSource = MathUtil.MapToWorld(Targets[0].Position);

            LerpCurrentPositionForTarget = MathUtil.MapToWorld(Targets[0].Position);
            LerpTargetPositionForTarget = MathUtil.MapToWorld(Source.Position);
            Camera.main.GetComponent<GameSceneCameraDriver>().NewTarget(Targets[0].Position);
            if(Source.IsPlayer)
            {
                Context.UIController.LootWindow.Hide();
            }
            var targetOldPosition = Targets[0].Position;
            var oldSourcePos = new Point(Source.Position.X, Source.Position.Y);
            var sourceDirection = MathUtil.RelativeDirection(Source.Position, targetOldPosition);
            if (Source.View != null && Source.View.SkeletonAnimation != null)
            {
                var skeletonAnimation = Source.View.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Walk, sourceDirection), true);
            }

            var targetDirection = MathUtil.RelativeDirection(Targets[0].Position, oldSourcePos);
            if (Targets[0].View != null && Targets[0].View.SkeletonAnimation != null)
            {
                var skeletonAnimation = Targets[0].View.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Walk, targetDirection), true);
            }
        }


        public override void Do()
        {
            base.Do();

            ElapsedMovementTime += Time.deltaTime;

            if (ElapsedMovementTime > Move.TimeBetweenTiles)
            {
                ElapsedMovementTime = Move.TimeBetweenTiles;
            }

            var lerpPercentarge = ElapsedMovementTime / Move.TimeBetweenTiles;

            var _lerpPosSrc = Vector2.Lerp(LerpCurrentPositionForSource, LerpTargetPositionForSource, lerpPercentarge);
            var _lerpPosTarget = Vector2.Lerp(LerpCurrentPositionForTarget, LerpTargetPositionForTarget, lerpPercentarge);

            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = _lerpPosSrc;
            }
            if (Targets[0].View != null && Targets[0].View.ViewGameObject != null)
            {
                Targets[0].View.ViewGameObject.transform.position = _lerpPosTarget;
            }
            if (Vector2.Distance(_lerpPosSrc, LerpTargetPositionForSource) < 0.005f)
            {
                isDoneInternal = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Context.EntitySystem.Deregister(Source, Context.GameStateManager.Game.CurrentLevel);
            Context.EntitySystem.Deregister(Targets[0], Context.GameStateManager.Game.CurrentLevel);

            var targetOldPosition = Targets[0].Position;
            var oldSourcePos = new Point(Source.Position.X, Source.Position.Y);


            // Move the view to the new position
            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = MathUtil.MapToWorld(Targets[0].Position);
            }
            if (Targets[0].View != null && Targets[0].View.ViewGameObject != null)
            {
                Targets[0].View.ViewGameObject.transform.position = MathUtil.MapToWorld(oldSourcePos);
            }

            // Actually set new position

            Source.Position = targetOldPosition;
            Targets[0].Position = oldSourcePos;

            Context.GameStateManager.Game.CurrentLevel.Grid[Source.Position].Walkable = !Source.BlocksPathing;
            Context.GameStateManager.Game.CurrentLevel.Grid[Targets[0].Position].Walkable = !Targets[0].BlocksPathing;

            Context.EntitySystem.Register(Source, Context.GameStateManager.Game.CurrentLevel);
            Context.EntitySystem.Register(Targets[0], Context.GameStateManager.Game.CurrentLevel);

            foreach (var potentialTrigger in Context.GameStateManager.Game.CurrentLevel.Entitys)
            {
                var onStepTriggers = potentialTrigger.GetEffects((effectInQuestion) => { return effectInQuestion.CanTriggerOnStep(); });
                foreach(var onStepTrigger in onStepTriggers)
                {
                    var points = MathUtil.GetPointsByOffset(potentialTrigger.Position, potentialTrigger.Trigger.Offsets);
                    CombatUtil.PerformTriggerStepAbilityIfSteppedOn(Source, potentialTrigger, points);
                    CombatUtil.PerformTriggerStepAbilityIfSteppedOn(Targets[0], potentialTrigger, points);
                }
            }

            if (Source.IsPlayer)
            {
                var level = Context.GameStateManager.Game.CurrentLevel;
                var entitiesInPos = level.Grid[Source.Position].EntitiesInPosition;
                Move.HandleInputHints(entitiesInPos);
            }
        }

        public override bool IsValid()
        {
            var adj = Source.Position.IsAdjacentTo(Targets[0].Position);
            var ort = Source.Position.IsOrthogonalTo(Targets[0].Position);
            return Source.Position.IsAdjacentTo(Targets[0].Position) && Source.Position.IsOrthogonalTo(Targets[0].Position);
        }
    }
}
