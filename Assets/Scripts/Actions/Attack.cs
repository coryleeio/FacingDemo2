using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    // Action which executes an attack with an item, spell, bare hands etc.
    // uses an attack capability to resolve the thing being attacked with, source etc.
    // basically this action applies an attackCapability.
    public class Attack : Action
    {
        private CalculatedAttack CalculatedAttack;

        // Book keeping values
        private AttackParameters AttackParameters;
        private AttackTypeParameters AttackTypeParameters;
        private ExplosionParameters ExplosionParameters;
        private ProjectileAppearance ProjectileAppearance;

        private float ElapsedTime = 0.0f;
        private GameObject ProjectileView;
        private int NextPointIndex = 0;
        private Point NextGridPosition;
        private Vector2 LerpCurrentPosition;
        private Vector2 LerpCurrentNextPosition;
        private bool isDoneInternal = false;

        private Attack() { }

        public Attack(CalculatedAttack calculatedAttack)
        {
            this.CalculatedAttack = calculatedAttack;
            AttackParameters = CombatUtil.AttackParametersResolve(this.CalculatedAttack.Source, this.CalculatedAttack.AttackType, this.CalculatedAttack.Item);
            AttackTypeParameters = CombatUtil.AttackTypeParametersResolve(this.CalculatedAttack.Source, this.CalculatedAttack.AttackType, this.CalculatedAttack.Item);
            if (AttackParameters != null)
            {
                ProjectileAppearance = AttackParameters.ProjectileAppearance;
                ExplosionParameters = AttackParameters.ExplosionParameters;
            }
        }

        public override bool IsValid()
        {
            return AttackTypeParameters != null && AttackParameters != null && CombatUtil.CanAttackWithItem(this.CalculatedAttack.Source, this.CalculatedAttack.AttackType, this.CalculatedAttack.Item);
        }

        public override void Enter()
        {
            base.Enter();
            Assert.IsNotNull(AttackParameters);
            Assert.IsNotNull(AttackTypeParameters);
            Assert.AreNotEqual(CalculatedAttack.AttackType, AttackType.NotSet);

            this.CalculatedAttack.Source.Direction = CalculatedAttack.DirectionOfAttack;
            CombatUtil.ApplyItemStateChanges(CalculatedAttack);
            Context.UIController.Refresh();

            if (this.CalculatedAttack.Source.View != null && this.CalculatedAttack.Source.View.SkeletonAnimation != null)
            {
                var skeletonAnimation = this.CalculatedAttack.Source.View.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();
                if (this.CalculatedAttack.AttackType == AttackType.Zapped)
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.CastStart, CalculatedAttack.DirectionOfAttack), false);
                }
                else if (this.CalculatedAttack.AttackType == AttackType.Ranged)
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Shoot, CalculatedAttack.DirectionOfAttack), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, CalculatedAttack.DirectionOfAttack), true, 5.0f);
                }
                else
                {
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Attack, CalculatedAttack.DirectionOfAttack), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, CalculatedAttack.DirectionOfAttack), true, 5.0f);
                }
            }
            BuildProjectileViewIfNeeded();
            MoveProjectileTowardNextPositionFromHere(MathUtil.MapToWorld(CalculatedAttack.Source.Position));

            // Remove the item from the view if it is the last item being thrown
            // and the character is not throwing an item from their inventory.
            if (CalculatedAttack.AttackType == AttackType.Thrown)
            {
                var itemBeingLaunched = GetItemBeingLaunched();
                if (itemBeingLaunched.NumberOfItems == 0)
                {
                    if (InventoryUtil.GetItemBySlot(CalculatedAttack.Source, ItemSlot.MainHand) == itemBeingLaunched)
                    {
                        InventoryUtil.UnequipItemInSlot(CalculatedAttack.Source, ItemSlot.MainHand);
                    }
                }
            }
        }

        private void MoveProjectileTowardNextPositionFromHere(Vector2 currentPosition)
        {
            if (NextGridPosition != null && ProjectileAppearance.OnLeaveDefinition != null)
            {
                ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnLeaveDefinition, NextGridPosition, CalculatedAttack.DirectionOfAttack, ProjectileView);
            }
            ElapsedTime = 0.0f;
            LerpCurrentPosition = currentPosition;
            NextGridPosition = CalculatedAttack.PointsAffectedByAttack[NextPointIndex];
            NextPointIndex++;
            LerpCurrentNextPosition = MathUtil.MapToWorld(NextGridPosition);
        }

        private bool IsEndPoint()
        {
            return NextPointIndex == CalculatedAttack.PointsAffectedByAttack.Count;
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
                perTileTravelTime = ProjectileAppearance.ProjectileDefinition.PerTileTravelTime;
            }

            if (AttackTypeParameters.AttackTargetingType == AttackTargetingType.SelectTarget)
            {
                // in this case - since we travel ALL the distance in 1 lerp, we want 
                // to set the perTileTravelTime to distance * the per tile travel time
                // to approximate how quickly to move the projectile
                // so that it will look consistent independent of range
                perTileTravelTime = perTileTravelTime * CalculatedAttack.Source.Position.Distance(NextGridPosition);
            }

            if (ElapsedTime > perTileTravelTime)
            {
                ElapsedTime = perTileTravelTime;
            }

            var lerpPercent = ElapsedTime / perTileTravelTime;
            if(LerpCurrentPosition != LerpCurrentNextPosition)
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

                if (ProjectileAppearance.OnEnterDefinition != null)
                {
                    ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnEnterDefinition, NextGridPosition, CalculatedAttack.DirectionOfAttack, ProjectileView);
                }

                foreach (var attackStateChanges in CalculatedAttack.AttackStateChanges)
                {
                    if (NextGridPosition == attackStateChanges.Target.Position)
                    {
                        if (ProjectileAppearance.OnHitDefinition != null)
                        {
                            ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnHitDefinition, NextGridPosition, CalculatedAttack.DirectionOfAttack, ProjectileView == null ? null : ProjectileView.gameObject);
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

            if (CalculatedAttack.AttackType == AttackType.Zapped && CalculatedAttack.Source.View != null && CalculatedAttack.Source.View.SkeletonAnimation != null)
            {
                var skeletonAnimation = CalculatedAttack.Source.View.SkeletonAnimation;
                skeletonAnimation.AnimationState.ClearTracks();
                skeletonAnimation.Skeleton.SetToSetupPose();

                skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.CastEnd, CalculatedAttack.DirectionOfAttack), false);
                skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, CalculatedAttack.DirectionOfAttack), true, 5.0f);
            }
            foreach(var stateChange in CalculatedAttack.SourceStateChanges)
            {
                CombatUtil.ApplyEntityStateChange(stateChange);
            }
            isDoneInternal = true;
            if (ExplosionParameters != null)
            {
                var explosionAction = new Explosion(CalculatedAttack);
                var currentStep = Context.FlowSystem.Steps.First.Value;
                var currentAction = currentStep.Actions.AddAfter(currentStep.Actions.First, explosionAction);
            }
            CombatUtil.ApplyGroundSpawnStateChange(CalculatedAttack);
        }

        private Item GetItemBeingLaunched()
        {
            var ammo = CombatUtil.AmmoResolve(CalculatedAttack.Source, CalculatedAttack.AttackType, CalculatedAttack.Item);
            return CalculatedAttack.AttackType == AttackType.Ranged ? ammo : CalculatedAttack.Item;
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
                ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.OnSwingDefinition, CalculatedAttack.Source.Position, CalculatedAttack.DirectionOfAttack, ProjectileView);
            }
            if (ProjectileAppearance.ProjectileDefinition != null)
            {
                ProjectileView = ViewFactory.InstantiateProjectileAppearance(ProjectileAppearance.ProjectileDefinition, CalculatedAttack.Source.Position, CalculatedAttack.DirectionOfAttack, ProjectileView);
            }
        }
    }
}
