using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Attack : Action
    {
        // Inputs
        private Direction Direction;

        // Book keeping values
        private float ElapsedTime = 0.0f;
        private GameObject ProjectileView;
        private int NumberOfTargetsHit;
        private int RangeTraversed = 0;
        private Point SourceGridPosition;
        private Point NextGridPosition;
        private Point LastNonWallTraversed;
        private Vector2 LerpCurrentPosition;
        private Vector2 LerpCurrentNextPosition;
        private Point AimedTarget;
        private bool isDoneInternal = false;
        private AttackParameters AttackParameters;
        private ExplosionParameters ExplosionParameters;
        private AttackCapability AttackCapability;
        private ProjectileAppearance ProjectileAppearance;

        private Attack() { }

        public Attack(AttackCapability attackCapability, Direction direction, Point aimedTarget)
        {
            this.AttackCapability = attackCapability;
            this.Direction = direction;
            this.AimedTarget = aimedTarget;

            if (AttackCapability != null)
            {
                this.AttackParameters = AttackCapability.AttackParameters;
            }
            if (AttackParameters != null)
            {
                ProjectileAppearance = AttackCapability.AttackParameters.ProjectileAppearance;
                ExplosionParameters = AttackCapability.AttackParameters.ExplosionParameters;
            }
        }

        public override bool IsValid()
        {
            return AttackCapability != null && AttackCapability.CanPerform;
        }

        public override void Enter()
        {
            base.Enter();
            Assert.IsNotNull(AttackCapability);
            Assert.AreNotEqual(AttackCapability.AttackType, AttackType.NotSet);

            AttackCapability.Source.Direction = Direction;
            if (AttackCapability.Source.View != null && AttackCapability.Source.View.SkeletonAnimation != null)
            {
                var skeletonAnimation = AttackCapability.Source.View.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                if (AttackCapability.AttackType == AttackType.Zapped)
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.CastStart, Direction), false);
                }
                else if (AttackCapability.AttackType == AttackType.Ranged)
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Shoot, Direction), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, Direction), true, 5.0f);
                }
                else
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Attack, Direction), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, Direction), true, 5.0f);
                }
            }

            SourceGridPosition = new Point(AttackCapability.Source.Position.X, AttackCapability.Source.Position.Y);
            LastNonWallTraversed = new Point(SourceGridPosition.X, SourceGridPosition.Y);
            NumberOfTargetsHit = 0;
            BuildProjectileViewIfNeeded();
            MoveProjectileTowardNextPositionFromHere(MathUtil.MapToWorld(AttackCapability.Source.Position));

            // Remove the item from the view if it is the last item being thrown
            // and the character is not throwing an item from their inventory.
            if (AttackCapability.AttackType == AttackType.Thrown)
            {
                var itemBeingLaunched = GetItemBeingLaunched(AttackCapability);
                if (itemBeingLaunched.NumberOfItems == 1)
                {
                    if (AttackCapability.Source.Inventory.GetItemBySlot(ItemSlot.MainHand) == itemBeingLaunched)
                    {
                        AttackCapability.Source.Inventory.UnequipItemInSlot(ItemSlot.MainHand);
                        Context.ViewFactory.BuildView(AttackCapability.Source);
                    }
                }
            }
        }

        private void MoveProjectileTowardNextPositionFromHere(Vector2 currentPosition)
        {
            if (ProjectileAppearance.OnLeaveDefinition != null)
            {
                ProjectileAppearance.OnLeaveDefinition.Instantiate(NextGridPosition, Direction, ProjectileView);
            }

            ElapsedTime = 0.0f;
            RangeTraversed++;
            LerpCurrentPosition = currentPosition;
            if (AttackCapability.AttackTargetingType == AttackTargetingType.Line)
            {
                NextGridPosition = SourceGridPosition + (MathUtil.OffsetForDirection(Direction) * RangeTraversed);
            }
            if (AttackCapability.AttackTargetingType == AttackTargetingType.SelectTarget)
            {
                NextGridPosition = AimedTarget;
            }
            LerpCurrentNextPosition = MathUtil.MapToWorld(NextGridPosition);


            if (Context.GameStateManager.Game.CurrentLevel.Grid[NextGridPosition].TileType == TileType.Floor || Context.GameStateManager.Game.CurrentLevel.Grid[NextGridPosition].TileType == TileType.Empty)
            {
                LastNonWallTraversed = new Point(NextGridPosition.X, NextGridPosition.Y);
            }
        }

        public override void Do()
        {
            base.Do();
            if (isDoneInternal)
            {
                // if this is not here, when applying spells to individual targets directly without the line fire
                // you will double tap them as do will be called once more than you will expect.
                return;
            }
            ElapsedTime += Time.deltaTime;
            var perTileTravelTime = 1.00f;

            if (ProjectileAppearance != null && ProjectileAppearance.ProjectileDefinition != null)
            {
                perTileTravelTime = ProjectileAppearance.ProjectileDefinition.PerTileTravelTime;
            }

            if (AttackCapability.AttackTargetingType == AttackTargetingType.SelectTarget)
            {
                // in this case - since we travel ALL the distance in 1 lerp, we want 
                // to set the perTileTravelTime to distance * the per tile travel time
                // to approximate how quickly to move the projectile
                // so that it will look consistent independent of range
                NextGridPosition = AimedTarget;
                perTileTravelTime = perTileTravelTime * SourceGridPosition.Distance(NextGridPosition);
            }

            if (ElapsedTime > perTileTravelTime)
            {
                ElapsedTime = perTileTravelTime;
            }

            var lerpPercent = ElapsedTime / perTileTravelTime;
            LerpCurrentPosition = Vector2.Lerp(LerpCurrentPosition, LerpCurrentNextPosition, lerpPercent);
            if (ProjectileView != null)
            {
                ProjectileView.gameObject.transform.position = LerpCurrentPosition;
            }
            if (Vector2.Distance(LerpCurrentPosition, LerpCurrentNextPosition) < 0.05f)
            {
                var level = Context.GameStateManager.Game.CurrentLevel;
                var targets = CombatUtil.HittableEntitiesInPositionsOnLevel(NextGridPosition, level);
                var hitTarget = targets.Count > 0;
                var hitWall = level.Grid.PointInGrid(NextGridPosition) && level.Grid[NextGridPosition].TileType == TileType.Wall;
                var hitMaxRange = RangeTraversed >= AttackCapability.Range;
                var movedThisPass = false;

                if (!hitWall && ProjectileAppearance.OnEnterDefinition != null)
                {
                    ProjectileAppearance.OnEnterDefinition.Instantiate(NextGridPosition, Direction, ProjectileView);
                }

                if (hitTarget)
                {
                    foreach (var target in targets)
                    {
                        if (ProjectileAppearance.OnHitDefinition != null)
                        {
                            ProjectileAppearance.OnHitDefinition.Instantiate(NextGridPosition, Direction, ProjectileView == null ? null : ProjectileView.gameObject);
                        }

                        NumberOfTargetsHit += targets.Count;
                        CombatUtil.ApplyEntityStateChange(AttackCapability.PerformOnTarget(target));
                        if (NumberOfTargetsHit == AttackCapability.NumberOfTargetsToPierce)
                        {
                            break;
                        }
                    }
                    if (AttackCapability.AttackTargetingType != AttackTargetingType.SelectTarget)
                    {
                        MoveProjectileTowardNextPositionFromHere(LerpCurrentNextPosition);
                    }
                    movedThisPass = true;
                    LastNonWallTraversed = new Point(targets[0].Position.X, targets[0].Position.Y);
                }

                var canHitMoreTargets = NumberOfTargetsHit < AttackCapability.NumberOfTargetsToPierce && AttackCapability.AttackTargetingType != AttackTargetingType.SelectTarget;

                if (!canHitMoreTargets || hitWall || hitMaxRange)
                {
                    if (AttackCapability.AttackType == AttackType.Zapped && AttackCapability.Source.View != null && AttackCapability.Source.View.SkeletonAnimation != null)
                    {
                        var skeletonAnimation = AttackCapability.Source.View.SkeletonAnimation;
                        skeletonAnimation.AnimationState.ClearTracks();
                        skeletonAnimation.Skeleton.SetToSetupPose();

                        skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.CastEnd, Direction), false);
                        skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, Direction), true, 5.0f);
                    }
                    isDoneInternal = true;
                    if (ExplosionParameters != null)
                    {
                        var explosionAction = new Explosion(AttackCapability.Source, AttackParameters, NextGridPosition);
                        var currentStep = Context.FlowSystem.Steps.First.Value;
                        var currentAction = currentStep.Actions.AddAfter(currentStep.Actions.First, explosionAction);
                    }
                }
                else if (!movedThisPass)
                {
                    MoveProjectileTowardNextPositionFromHere(LerpCurrentNextPosition);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            if (AttackCapability.Source.Body.MeleeParameters.Count == 0)
            {
                throw new NotImplementedException("You don't have any attacks, but are trying to attack anyway?");
            }
            if (ProjectileView != null)
            {
                GameObject.Destroy(ProjectileView.gameObject);
            }
            HandleSpawnItemOnGroundIfRelevant();
        }

        private void HandleSpawnItemOnGroundIfRelevant()
        {
            if (AttackCapability.AttackType == AttackType.Ranged || AttackCapability.AttackType == AttackType.Thrown)
            {
                Item itemBeingLaunched = GetItemBeingLaunched(AttackCapability);
                var shouldSpawnItemOnGround = MathUtil.PercentageChanceEventOccurs(itemBeingLaunched.ChanceToSurviveLaunch);
                if (shouldSpawnItemOnGround)
                {
                    var placeItemLanded = LastNonWallTraversed;
                    var groundDrop = EntityFactory.Build(UniqueIdentifier.ENTITY_GROUND_DROP);
                    groundDrop.Position = new Point(placeItemLanded.X, placeItemLanded.Y);
                    groundDrop.Name = itemBeingLaunched.DisplayName;
                    var level = Context.GameStateManager.Game.CurrentLevel;
                    Context.EntitySystem.Register(groundDrop, level);
                    var itemCopy = ItemFactory.Build(itemBeingLaunched.UniqueIdentifier);
                    itemCopy.NumberOfItems = 1;
                    groundDrop.Inventory.AddItem(itemCopy);
                    Context.ViewFactory.BuildView(groundDrop);
                }
                AttackCapability.Source.Inventory.ConsumeItem(itemBeingLaunched);
            }

        }

        private Item GetItemBeingLaunched(AttackCapability capability)
        {
            return AttackCapability.AttackType == AttackType.Ranged ? capability.Ammo : capability.Item;
        }

        public override bool IsEndable
        {
            get
            {
                return isDoneInternal;
            }
        }

        private void BuildProjectileViewIfNeeded()
        {
            if (ProjectileAppearance.OnSwingDefinition != null)
            {
                ProjectileAppearance.OnSwingDefinition.Instantiate(SourceGridPosition, Direction, ProjectileView);
            }
            if (ProjectileAppearance.ProjectileDefinition != null)
            {
                ProjectileView = ProjectileAppearance.ProjectileDefinition.Instantiate(SourceGridPosition, Direction, ProjectileView);
            }
        }
    }
}
