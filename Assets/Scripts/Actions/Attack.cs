using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public enum CombatContext
    {
        NotSet,
        Melee,
        Ranged,
        Thrown,
        Zapped,
        OnUse,
    }

    public class Attack : Action
    {
        // Inputs
        private AttackCapabilities AttackCapabilities;
        private CombatContext CombatContext;
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
        private AttackCapability CombatContextCapability;
        private ProjectileAppearance ProjectileAppearance;

        private Attack() { }

        public Attack(AttackCapabilities attackCapabilities, CombatContext combatContext, Direction direction, Point aimedTarget)
        {
            this.AttackCapabilities = attackCapabilities;
            this.CombatContext = combatContext;
            this.Direction = direction;
            this.AimedTarget = aimedTarget;

            this.CombatContextCapability = attackCapabilities[CombatContext];
            if (CombatContextCapability != null)
            {
                this.AttackParameters = CombatContextCapability.AttackParameters;
            }
            if (AttackParameters != null)
            {
                ProjectileAppearance = CombatContextCapability.AttackParameters.ProjectileAppearance;
                ExplosionParameters = CombatContextCapability.AttackParameters.ExplosionParameters;
            }
        }

        public override int TimeCost
        {
            get
            {
                return 250;
            }
        }

        public override bool IsValid()
        {
            return AttackCapabilities != null && AttackCapabilities[CombatContext].CanPerform;
        }

        public override void Enter()
        {
            base.Enter();
            Assert.IsNotNull(AttackCapabilities);

            CombatContextCapability.Source.Direction = Direction;
            if (CombatContextCapability.Source.View != null && CombatContextCapability.Source.View.SkeletonAnimation != null)
            {
                var skeletonAnimation = CombatContextCapability.Source.View.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                if (CombatContext == CombatContext.Zapped)
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.CastStart, Direction), false);
                }
                else if (CombatContext == CombatContext.Ranged)
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

            SourceGridPosition = new Point(CombatContextCapability.Source.Position.X, CombatContextCapability.Source.Position.Y);
            LastNonWallTraversed = new Point(SourceGridPosition.X, SourceGridPosition.Y);
            NumberOfTargetsHit = 0;
            BuildProjectileViewIfNeeded();
            MoveProjectileTowardNextPositionFromHere(MathUtil.MapToWorld(CombatContextCapability.Source.Position));

            // Remove the item from the view if it is the last item being thrown
            // and the character is not throwing an item from their inventory.
            if (CombatContext == CombatContext.Thrown)
            {
                var capability = AttackCapabilities[CombatContext];
                var itemBeingLaunched = GetItemBeingLaunched(capability);
                if (itemBeingLaunched.NumberOfItems == 1)
                {
                    if (capability.Source.Inventory.GetItemBySlot(ItemSlot.MainHand) == itemBeingLaunched)
                    {
                        capability.Source.Inventory.UnequipItemInSlot(ItemSlot.MainHand);
                        Context.ViewFactory.BuildView(AttackCapabilities.Source);
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
            if (CombatContextCapability.AttackTargetingType == AttackTargetingType.Line)
            {
                NextGridPosition = SourceGridPosition + (MathUtil.OffsetForDirection(Direction) * RangeTraversed);
            }
            if (CombatContextCapability.AttackTargetingType == AttackTargetingType.SelectTarget)
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

            if (CombatContextCapability.AttackTargetingType == AttackTargetingType.SelectTarget)
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
                var hitMaxRange = RangeTraversed >= CombatContextCapability.Range;
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
                        CombatUtil.ApplyEntityStateChange(CombatContextCapability.PerformOnTarget(target));
                        if (NumberOfTargetsHit == CombatContextCapability.NumberOfTargetsToPierce)
                        {
                            break;
                        }
                    }
                    if (CombatContextCapability.AttackTargetingType != AttackTargetingType.SelectTarget)
                    {
                        MoveProjectileTowardNextPositionFromHere(LerpCurrentNextPosition);
                    }
                    movedThisPass = true;
                    LastNonWallTraversed = new Point(targets[0].Position.X, targets[0].Position.Y);
                }

                var canHitMoreTargets = NumberOfTargetsHit < CombatContextCapability.NumberOfTargetsToPierce && CombatContextCapability.AttackTargetingType != AttackTargetingType.SelectTarget;

                if (!canHitMoreTargets || hitWall || hitMaxRange)
                {
                    if (CombatContext == CombatContext.Zapped && CombatContextCapability.Source.View != null && CombatContextCapability.Source.View.SkeletonAnimation != null)
                    {
                        var skeletonAnimation = CombatContextCapability.Source.View.SkeletonAnimation;
                        skeletonAnimation.AnimationState.ClearTracks();
                        skeletonAnimation.Skeleton.SetToSetupPose();

                        skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.CastEnd, Direction), false);
                        skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, Direction), true, 5.0f);
                    }
                    isDoneInternal = true;
                    if (ExplosionParameters != null)
                    {
                        var explosionAction = new Explosion(CombatContextCapability.Source, AttackParameters, NextGridPosition);
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
            if (CombatContextCapability.Source.Body.MeleeParameters.Count == 0)
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
            if (CombatContext == CombatContext.Ranged || CombatContext == CombatContext.Thrown)
            {
                Item itemBeingLaunched = GetItemBeingLaunched(CombatContextCapability);
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
                CombatContextCapability.Source.Inventory.ConsumeItem(itemBeingLaunched);
            }

        }

        private Item GetItemBeingLaunched(AttackCapability capability)
        {
            return CombatContext == CombatContext.Ranged ? capability.Ammo : capability.MainHand;
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
