using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Move : Action
    {
        [JsonIgnore]
        public override Entity Source
        {
            get; set;
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
            var newDirection = MathUtil.RelativeDirection(Source.Position, TargetPosition);

            var sortable = Source.Sortable;
            if (sortable != null && TargetPosition != null && newDirection == Direction.SouthEast || newDirection == Direction.SouthWest || newDirection == Direction.East || newDirection == Direction.South)
            {
                // Sort these from the higher index position immediately before moving them so they are sorted
                // corectly. Other directions should keep the higher index until the move is complete.
                sortable.Position = TargetPosition;
            }

            Source.Direction = newDirection;

            if (Source.SkeletonAnimation != null)
            {
                var skeletonAnimation = Source.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Walk, newDirection), false);
            }

            if (Source.IsPlayer)
            {
                Camera.main.GetComponent<GameSceneCameraDriver>().NewTarget(TargetPosition);
                Context.UIController.LootWindow.Hide();
            }
            var player = Context.Game.CurrentLevel.Player;
            var playerVision = player == null ? 10 : player.CalculateValueOfAttribute(Attributes.VisionRadius);
            if(player != null && Source.Position.Distance(player.Position) > playerVision && TargetPosition.Distance(player.Position) > playerVision)
            {
                isDoneInternal = true;
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

            if (Source.ViewGameObject != null)
            {
                Source.ViewGameObject.transform.position = _lerpPos;
            }
            if (Vector2.Distance(_lerpPos, targetVectorPos) < 0.005f)
            {
                isDoneInternal = true;
            }
        }

        public override void Exit()
        {
            base.Exit();

            // Unindex old position
            Context.EntitySystem.Deregister(Source, Context.Game.CurrentLevel);

            // Move the view to the new position
            if (Source.ViewGameObject != null)
            {
                Source.ViewGameObject.transform.position = MathUtil.MapToWorld(TargetPosition);
            }
            
            // Write new position to state
            Source.Position = TargetPosition;

            var sortable = Source.Sortable;
            if (sortable != null)
            {
                sortable.Position = TargetPosition;
            }

            // Index new position
            Context.EntitySystem.Register(Source, Context.Game.CurrentLevel);
            Context.VisibilitySystem.UpdateVisibility();
            CombatUtil.DoStepTriggersForMover(Source);

            if(Source.IsPlayer)
            {
                var level = Context.Game.CurrentLevel;
                ShowInputHintsForPressTrigger(Source);
            }
        }

        public static void ShowInputHintsForPressTrigger(Entity source)
        {
            var level = Context.Game.CurrentLevel;
            var grid = level.Grid;
            var entities = level.Entitys;
            var entitiesInPositionOfMove = grid[source.Position];
            var pressTriggers = entities.FindAll(Filters.PressTriggers);
            var anyHits = false;
            foreach(var trigger in pressTriggers)
            {
                if(Trigger.EntityInTrigger(source, trigger))
                {
                    if(trigger.Trigger.CanPerform(source, trigger))
                    {
                        Context.UIController.InputHint.ShowText(trigger.Trigger.Template.PressInputHint.Localize());
                        anyHits = true;
                    }
                }
            }

            if(!anyHits)
            {
                Context.UIController.InputHint.Hide();
            }
        }

        public override bool IsValid()
        {
            return (!Source.BlocksPathing || (Source.BlocksPathing && Context.Game.CurrentLevel.Grid[TargetPosition].Walkable && Context.Game.CurrentLevel.Grid[TargetPosition].EntitiesInPosition.FindAll(Filters.HittableEntities).Count == 0));
        }
    }
}
