using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class SwapPositionsWithAlly : Action
    {
        [JsonIgnore]
        public override Entity Source
        {
            get; set;
        }

        [JsonIgnore]
        public List<Entity> Targets = new List<Entity>(0);

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
            if (Source.SkeletonAnimation != null)
            {
                var skeletonAnimation = Source.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Walk, sourceDirection), true);
                Source.Direction = sourceDirection;
            }

            var targetDirection = MathUtil.RelativeDirection(Targets[0].Position, oldSourcePos);
            if (Targets[0].SkeletonAnimation != null)
            {
                var skeletonAnimation = Targets[0].SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Walk, targetDirection), true);
                Targets[0].Direction = targetDirection;
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

            if (Source.ViewGameObject != null)
            {
                Source.ViewGameObject.transform.position = _lerpPosSrc;
            }
            if (Targets[0].ViewGameObject != null)
            {
                Targets[0].ViewGameObject.transform.position = _lerpPosTarget;
            }
            if (Vector2.Distance(_lerpPosSrc, LerpTargetPositionForSource) < 0.005f)
            {
                isDoneInternal = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Context.EntitySystem.Deregister(Source, Context.Game.CurrentLevel);
            Context.EntitySystem.Deregister(Targets[0], Context.Game.CurrentLevel);

            var targetOldPosition = Targets[0].Position;
            var oldSourcePos = new Point(Source.Position.X, Source.Position.Y);


            // Move the view to the new position
            if ( Source.ViewGameObject != null)
            {
                Source.ViewGameObject.transform.position = MathUtil.MapToWorld(Targets[0].Position);
            }
            if (Targets[0].ViewGameObject != null)
            {
                Targets[0].ViewGameObject.transform.position = MathUtil.MapToWorld(oldSourcePos);
            }

            // Actually set new position
            Source.Position = targetOldPosition;
            var sourceSortable = Source.Sortable;
            if (sourceSortable != null)
            {
                sourceSortable.Position = targetOldPosition;
            }

            Targets[0].Position = oldSourcePos;
            var targetSortable = Targets[0].Sortable;
            if (targetSortable != null)
            {
                targetSortable.Position = oldSourcePos;
            }

            // Lock new positions
            Context.EntitySystem.Register(Source, Context.Game.CurrentLevel);
            Context.EntitySystem.Register(Targets[0], Context.Game.CurrentLevel);
            Context.VisibilitySystem.UpdateVisibility();

            CombatUtil.DoStepTriggersForMover(Source);
            CombatUtil.DoStepTriggersForMover(Targets[0]);

            if (Source.IsPlayer)
            {
                var level = Context.Game.CurrentLevel;
                Move.ShowInputHintsForPressTrigger(Source);
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
