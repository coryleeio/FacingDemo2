using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    // Action which executes an attack with an item, spell, bare hands etc.
    // uses an attack capability to resolve the thing being attacked with, source etc.
    // basically this action applies an attackCapability.
    public class CombatAction : Action
    {
        private CalculatedCombatAction CalculatedCombatAction;
        public override Entity Source {
            get
            {
                return CalculatedCombatAction.Source;
            }
            set
            {
                CalculatedCombatAction.Source = value;
            }
        }

        private ResolvedCombatActionDescriptor ResolvedCombatActionParameters
        {
            get
            {
                return CalculatedCombatAction.ResolvedCombatActionParameters;
            }
        }

        private ProjectileAppearanceTemplate ProjectileAppearance
        {
            get
            {
                if (CalculatedCombatAction != null && CalculatedCombatAction.ResolvedCombatActionParameters != null && CalculatedCombatAction.ResolvedCombatActionParameters.CombatActionParameters != null)
                {
                    return CalculatedCombatAction.ResolvedCombatActionParameters.CombatActionParameters.ProjectileAppearance;
                }
                return null;
            }
        }

        private float ElapsedTime = 0.0f;
        private GameObject ProjectileView;
        private int NextPointIndex = 0;
        private Point NextGridPosition;
        private Vector2 LerpCurrentPosition;
        private Vector2 LerpCurrentNextPosition;
        private bool isDoneInternal = false;

        private CombatAction() { }

        public CombatAction(CalculatedCombatAction calculatedAttack)
        {
            this.CalculatedCombatAction = calculatedAttack;
        }

        public override bool IsValid()
        {
            return ResolvedCombatActionParameters != null && ResolvedCombatActionParameters.CombatActionParameters != null && CombatUtil.CanAttackWithItem(this.CalculatedCombatAction.Source, this.CalculatedCombatAction.CombatActionType, this.CalculatedCombatAction.Item);
        }

        public override void Enter()
        {
            base.Enter();
            Assert.IsNotNull(ResolvedCombatActionParameters);
            Assert.AreNotEqual(CalculatedCombatAction.CombatActionType, CombatActionType.NotSet);

            this.CalculatedCombatAction.Source.Direction = CalculatedCombatAction.DirectionOfAttack;
            CombatUtil.ApplyItemStateChanges(CalculatedCombatAction);
            Context.UIController.Refresh();
            var player = Context.Game.CurrentLevel.Player;
            var targetsThePlayerCanSee = CalculatedCombatAction.AttackStateChanges.FindAll((v) => player.CanSee(v.Target.Position));

            if(this.CalculatedCombatAction.CombatActionType == CombatActionType.Melee && targetsThePlayerCanSee.Count == 0)
            {
                // skip offscreen melee attacks
                isDoneInternal = true;
                CombatUtil.ApplyAttackInstantly(CalculatedCombatAction);
            }

            if (this.CalculatedCombatAction.Source.SkeletonAnimation != null)
            {
                var skeletonAnimation = this.CalculatedCombatAction.Source.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                if (this.CalculatedCombatAction.CombatActionType == CombatActionType.Zapped)
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.CastStart, CalculatedCombatAction.DirectionOfAttack), false);
                }
                else if (this.CalculatedCombatAction.CombatActionType == CombatActionType.Ranged)
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Shoot, CalculatedCombatAction.DirectionOfAttack), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Idle, CalculatedCombatAction.DirectionOfAttack), true, 5.0f);
                }
                else
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Attack, CalculatedCombatAction.DirectionOfAttack), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Idle, CalculatedCombatAction.DirectionOfAttack), true, 5.0f);
                }
            }
            BuildProjectileViewIfNeeded();
            MoveProjectileTowardNextPositionFromHere(MathUtil.MapToWorld(CalculatedCombatAction.Source.Position));

            // Remove the item from the view if it is the last item being thrown
            // and the character is not throwing an item from their inventory.
            if (CalculatedCombatAction.CombatActionType == CombatActionType.Thrown)
            {
                var itemBeingLaunched = GetItemBeingLaunched();
                if (itemBeingLaunched.NumberOfItems == 0)
                {
                    if (InventoryUtil.GetItemBySlot(CalculatedCombatAction.Source, ItemSlot.MainHand) == itemBeingLaunched)
                    {
                        InventoryUtil.UnequipItemInSlot(CalculatedCombatAction.Source, ItemSlot.MainHand);
                    }
                }
            }
        }

        private void MoveProjectileTowardNextPositionFromHere(Vector2 currentPosition)
        {
            if (NextGridPosition != null && ProjectileAppearance.OnLeaveDefinition != null)
            {
                ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnLeaveDefinition, NextGridPosition, CalculatedCombatAction.DirectionOfAttack, ProjectileView);
            }
            ElapsedTime = 0.0f;
            LerpCurrentPosition = currentPosition;
            NextGridPosition = CalculatedCombatAction.PointsAffectedByAttack[NextPointIndex];
            NextPointIndex++;
            LerpCurrentNextPosition = MathUtil.MapToWorld(NextGridPosition);
        }

        private bool IsEndPoint()
        {
            return NextPointIndex == CalculatedCombatAction.PointsAffectedByAttack.Count;
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
            var perTileTravelTime = 0.5f;

            if (ProjectileAppearance != null && ProjectileAppearance.ProjectileDefinition != null)
            {
                perTileTravelTime = ProjectileAppearance.ProjectileDefinition.TimeSpentInEachTile;
            }

            if (ResolvedCombatActionParameters.CombatActionParameters.TargetingType == CombatActionTargetingType.SelectTarget)
            {
                // in this case - since we travel ALL the distance in 1 lerp, we want 
                // to set the perTileTravelTime to distance * the per tile travel time
                // to approximate how quickly to move the projectile
                // so that it will look consistent independent of range
                perTileTravelTime = perTileTravelTime * CalculatedCombatAction.Source.Position.Distance(NextGridPosition);
            }

            if (ElapsedTime > perTileTravelTime)
            {
                ElapsedTime = perTileTravelTime;
            }

            var lerpPercent = ElapsedTime / perTileTravelTime;
            if (LerpCurrentPosition != LerpCurrentNextPosition)
            {
                // When applying to self this will cause an error
                LerpCurrentPosition = Vector2.Lerp(LerpCurrentPosition, LerpCurrentNextPosition, lerpPercent);
                if (ProjectileView != null)
                {
                    ProjectileView.gameObject.transform.position = LerpCurrentPosition;
                }
            }

            if (Vector2.Distance(LerpCurrentPosition, LerpCurrentNextPosition) < 0.05f)
            {
                var level = Context.Game.CurrentLevel;

                if (ProjectileAppearance != null && ProjectileAppearance.OnEnterDefinition != null)
                {
                    ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnEnterDefinition, NextGridPosition, CalculatedCombatAction.DirectionOfAttack, ProjectileView);
                }

                foreach (var attackStateChanges in CalculatedCombatAction.AttackStateChanges)
                {
                    if (NextGridPosition == attackStateChanges.Target.Position)
                    {
                        if (ProjectileAppearance != null && ProjectileAppearance.OnHitDefinition != null)
                        {
                            ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnHitDefinition, NextGridPosition, CalculatedCombatAction.DirectionOfAttack, ProjectileView == null ? null : ProjectileView.gameObject);
                        }
                        CombatUtil.ApplyEntityStateChange(attackStateChanges);
                    }
                }
                if (IsEndPoint())
                {
                    isDoneInternal = true;
                }
                else
                {
                    MoveProjectileTowardNextPositionFromHere(LerpCurrentPosition);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
            if (ProjectileView != null)
            {
                GameObject.Destroy(ProjectileView.gameObject);
            }

            if (CalculatedCombatAction.CombatActionType == CombatActionType.Zapped && CalculatedCombatAction.Source.SkeletonAnimation != null)
            {
                var skeletonAnimation = CalculatedCombatAction.Source.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();

                skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.CastEnd, CalculatedCombatAction.DirectionOfAttack), false);
                skeletonAnimation.AnimationState.AddAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Idle, CalculatedCombatAction.DirectionOfAttack), true, 5.0f);
            }
            foreach (var stateChange in CalculatedCombatAction.SourceStateChanges)
            {
                CombatUtil.ApplyEntityStateChange(stateChange);
            }
            isDoneInternal = true;
            if (ResolvedCombatActionParameters.ExplosionParameters != null)
            {
                var explosionAction = new Explosion(CalculatedCombatAction);
                var currentStep = Context.FlowSystem.Steps.First.Value;
                var currentAction = currentStep.Actions.AddAfter(currentStep.Actions.First, explosionAction);
            }
            CombatUtil.ApplyGroundSpawnStateChange(CalculatedCombatAction);
        }

        private Item GetItemBeingLaunched()
        {
            var ammo = CombatUtil.AmmoResolve(CalculatedCombatAction.Source, CalculatedCombatAction.CombatActionType, CalculatedCombatAction.Item);
            return CalculatedCombatAction.CombatActionType == CombatActionType.Ranged ? ammo : CalculatedCombatAction.Item;
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
            if (ProjectileAppearance != null && ProjectileAppearance.ProjectileDefinition != null)
            {
                ProjectileView = ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.ProjectileDefinition, CalculatedCombatAction.Source.Position, CalculatedCombatAction.DirectionOfAttack, ProjectileView, CalculatedCombatAction.Item.Template.ItemAppearance.GroundSprite);
            }
            if (ProjectileAppearance != null && ProjectileAppearance.OnSwingDefinition != null)
            {
                ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnSwingDefinition, CalculatedCombatAction.Source.Position, CalculatedCombatAction.DirectionOfAttack, ProjectileView);
            }
        }
    }
}
