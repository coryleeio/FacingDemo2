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

        private CombatContextCapability CombatContextCapability
        {
            get
            {
                return AttackCapabilities[CombatContext];
            }
        }

        // Book keeping values
        private float ElapsedTime = 0.0f;
        private float Duration = 0.5f;
        private GameObject ProjectileView;
        private int NumberOfTargetsHit;
        private int RangeTraversed = 0;
        private Point SourceGridPosition;
        private Point NextGridPosition;
        private Point LastNonWallTraversed;
        private Vector2 LerpCurrentPosition;
        private Vector2 LerpCurrentNextPosition;
        private bool isDoneInternal = false;

        private Attack() { }

        public Attack(AttackCapabilities attackCapabilities, CombatContext combatContext, Direction direction)
        {
            this.AttackCapabilities = attackCapabilities;
            this.CombatContext = combatContext;
            this.Direction = direction;
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
            SourceGridPosition = new Point(CombatContextCapability.Source.Position.X, CombatContextCapability.Source.Position.Y);
            LastNonWallTraversed = new Point(SourceGridPosition.X, SourceGridPosition.Y);
            NumberOfTargetsHit = 0;
            BuildProjectileViewIfNeeded();
            MoveProjectileTowardNextPositionFromHere(MathUtil.MapToWorld(CombatContextCapability.Source.Position));
        }

        private void MoveProjectileTowardNextPositionFromHere(Vector2 currentPosition)
        {
            ElapsedTime = 0.0f;
            RangeTraversed++;
            LerpCurrentPosition = currentPosition;
            NextGridPosition = SourceGridPosition + (MathUtil.OffsetForDirection(Direction) * RangeTraversed);
            LerpCurrentNextPosition = MathUtil.MapToWorld(NextGridPosition);
            if(Context.GameStateManager.Game.CurrentLevel.Grid[NextGridPosition].TileType == TileType.Floor || Context.GameStateManager.Game.CurrentLevel.Grid[NextGridPosition].TileType == TileType.Empty)
            {
                LastNonWallTraversed = new Point(NextGridPosition.X, NextGridPosition.Y);
            }
        }

        public override void Do()
        {
            base.Do();
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime > Duration)
            {
                ElapsedTime = Duration;
            }

            var lerpPercent = ElapsedTime / Duration;
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
                var hitWall = level.Grid.PointInGrid(NextGridPosition) && !level.Grid[NextGridPosition].Walkable;
                var hitMaxRange = RangeTraversed > CombatContextCapability.Range;

                if (hitTarget)
                {
                    foreach (var target in targets)
                    {
                        if (CombatContextCapability.OnHitPrefabs.Count > 0)
                        {
                            foreach (var onHitPrefab in CombatContextCapability.OnHitPrefabs)
                            {
                                if (onHitPrefab != null)
                                {
                                    var onHitInstance = GameObject.Instantiate<GameObject>(onHitPrefab);
                                    onHitInstance.transform.position = MathUtil.MapToWorld(target.Position);
                                }
                            }
                        }

                        NumberOfTargetsHit += targets.Count;
                        CombatUtil.ApplyEntityStateChange(CombatContextCapability.AttackTarget(target));
                        if (NumberOfTargetsHit == CombatContextCapability.NumberOfTargetsToPierce)
                        {
                            break;
                        }
                    }
                    MoveProjectileTowardNextPositionFromHere(LerpCurrentNextPosition);
                    LastNonWallTraversed = new Point(targets[0].Position.X, targets[0].Position.Y);
                }
                var canHitMoreTargets = NumberOfTargetsHit < CombatContextCapability.NumberOfTargetsToPierce;
                if (!canHitMoreTargets || hitWall || hitMaxRange)
                {
                    isDoneInternal = true;
                }
                else
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
                var capability = AttackCapabilities[CombatContext];
                Item itemBeingLaunched = GetItemBeingLaunched(capability);
                var shouldSpawnItemOnGround = MathUtil.PercentageChanceEventOccurs(itemBeingLaunched.ChanceToSurviveLaunch);
                if (shouldSpawnItemOnGround)
                {
                    var placeItemLanded = LastNonWallTraversed;
                    var groundDrop = Context.PrototypeFactory.BuildEntity(UniqueIdentifier.ENTITY_GROUND_DROP);
                    groundDrop.Position = new Point(placeItemLanded.X, placeItemLanded.Y);
                    groundDrop.Name = itemBeingLaunched.DisplayName;
                    var level = Context.GameStateManager.Game.CurrentLevel;
                    Context.EntitySystem.Register(groundDrop, level);
                    var itemCopy = ItemFactory.Build(itemBeingLaunched.UniqueIdentifier);
                    itemCopy.NumberOfItems = 1;
                    groundDrop.Inventory.AddItem(itemCopy);
                    Context.PrototypeFactory.BuildView(groundDrop);
                }
                capability.Source.Inventory.ConsumeItem(itemBeingLaunched);
            }

        }

        private Item GetItemBeingLaunched(CombatContextCapability capability)
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
            if (CombatContextCapability.OnSwingPrefabs.Count > 0)
            {
                foreach (var onSwingPrefab in CombatContextCapability.OnSwingPrefabs)
                {
                    if (onSwingPrefab != null)
                    {
                        var onSwingInstance = GameObject.Instantiate<GameObject>(onSwingPrefab);
                        onSwingInstance.transform.position = MathUtil.MapToWorld(CombatContextCapability.Source.Position);
                    }
                }
            }
            if (CombatContextCapability.ProjectilePrefab)
            {
                ProjectileView = GameObject.Instantiate<GameObject>(CombatContextCapability.ProjectilePrefab);
                ProjectileView.transform.eulerAngles = MathUtil.GetProjectileRotation(Direction);
                var projectileItem = GetItemBeingLaunched(AttackCapabilities[CombatContext]);
                if(projectileItem.ItemAppearance.ShouldSpinWhenThrown)
                {
                    ProjectileView.AddComponent<ProjectileRotator>();
                }
            }
        }
    }
}
